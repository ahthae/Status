import styles from './StatusGraph.module.scss'
import { LineChart, Line, ResponsiveContainer, XAxis, YAxis } from 'recharts';

function parseTimeSpan(timeSpanStr) {
    const timeSplit = timeSpanStr.split(':');
    const seconds = parseInt(timeSplit[0]) * 3600 + parseInt(timeSplit[1]) * 60 + parseFloat(timeSplit[2]);
    return seconds * 1000;
}

export default function StatusGraph({ server }) {
    let minResponseTime, maxResponseTime;
    const data = server.responses.map(response => {
        const responseTime = parseTimeSpan(response.responseTime);

        if (minResponseTime === undefined) minResponseTime = responseTime;
        else if (responseTime < minResponseTime) minResponseTime = responseTime;
        if (maxResponseTime === undefined) maxResponseTime = responseTime;
        else if (responseTime > maxResponseTime) maxResponseTime = responseTime;

        return {
            name: new Date(response.timestamp).toTimeString(),
            value: responseTime
        }
    });

    return (
        <div className={styles.statusGraph}>
            <h3 className={styles.url}>{new URL(server.url).host}</h3>
            <ResponsiveContainer>
                <LineChart data={data}>
                    <Line 
                        className={styles.line}
                        type="monotone" 
                        dataKey="value"
                    />
                    <XAxis tick={false} />
                    <YAxis 
                        axisLine={false} 
                        tickLine={false} 
                        ticks={[Math.floor(minResponseTime), Math.floor(maxResponseTime)]} 
                        unit="ms"
                    />
                </LineChart>
            </ResponsiveContainer>
        </div>
    );
}