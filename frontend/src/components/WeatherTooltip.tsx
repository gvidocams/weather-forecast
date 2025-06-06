import { TooltipProps } from 'recharts';
import {WeatherData} from "../types/WeatherData";

export const WeatherTooltip = ({ active, payload }: TooltipProps<number, string>) => {
    if (!active || !payload || !payload.length) return null;

    const data = payload[0].payload as WeatherData;

    return (
        <div style={{ background: '#fff', border: '1px solid #ccc', padding: '10px' }}>
            <strong>{data.city}, {data.country}</strong>
            <p>Current: {data.temperature}°C</p>
            <p>Min: {data.minTemperature}°C</p>
            <p>Max: {data.maxTemperature}°C</p>
            <p>Updated: {new Date(data.lastUpdateTime).toLocaleString()}</p>
        </div>
    );
};