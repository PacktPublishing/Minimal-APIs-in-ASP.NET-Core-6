import http from "k6/http";
import { check } from "k6";
import { Rate } from 'k6/metrics';

const errorRate = new Rate('errorRate');
export let options = {
    summaryTrendStats: ["avg", "p(95)"],
    stages: [
        // Linearly ramp up from 1 to 50 VUs during 10 seconds
        { target: 50, duration: "10s" },
        // Hold at 50 VUs for the next 1 minute
        { target: 50, duration: "1m" },
        // Linearly ramp down from 50 to 0 VUs over the last 15 seconds
        { target: 0, duration: "15s" }
    ],
    thresholds: {
        // We want the 95th percentile of all HTTP request durations to be less than 500ms
        "http_req_duration": ["p(95)<500"],
        // Requests with the staticAsset tag should finish even faster
        "http_req_duration{staticAsset:yes}": ["p(99)<250"],
        // Thresholds based on the custom metric we defined and use to track application failures
        errorRate: [
            // more than 5% of errors will abort the test
            { threshold: 'rate < 0.05', abortOnFail: true, delayAbortEval: '1m' },
        ],
    },
};

export default function () {
    let body = JSON.stringify({ id: 0, description: 'test' });
    let res = http.post("http://localhost:7060/validations", body, { headers: { "Content-Type": "application/json" } });

    let j = JSON.parse(res.body);

    // Verify response
    check(res, {
        "status is 200": (r) => r.status === 200,
        "is key correct": (r) => j.id === 0,
    });
}