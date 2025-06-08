import React, { useState, useEffect } from 'react';
import { WeatherLogApiResponse } from '../types/WeatherLogApiResponse';

const WeatherLogList: React.FC = () => {
    const [weatherLogs, setWeatherLogs] = useState<WeatherLogApiResponse[]>([]);
    const [loading, setLoading] = useState<boolean>(true);
    const [error, setError] = useState<string | null>(null);

    useEffect(() => {
        const fetchWeatherLogs = async () => {
            try {
                setLoading(true);
                const response = await fetch('http://localhost:5218/api/weather/retrieval-logs');

                if (!response.ok) {
                    throw new Error(`HTTP error! status: ${response.status}`);
                }

                const apiData: WeatherLogApiResponse[] = await response.json();
                setWeatherLogs(apiData);
                setError(null);
            } catch (err) {
                setError(err instanceof Error ? err.message : 'An error occurred');
                console.error('Error fetching weather logs:', err);
            } finally {
                setLoading(false);
            }
        };

        fetchWeatherLogs();
    }, []);

    if (loading) {
        return <div>Loading weather logs...</div>;
    }

    if (error) {
        return (
            <div>
                <p>Error loading weather logs: {error}</p>
                <button onClick={() => window.location.reload()}>Retry</button>
            </div>
        );
    }

    if (weatherLogs.length === 0) {
        return <div>No weather logs found.</div>;
    }

    return (
            <table>
                <thead>
                <tr>
                    <th>Update Date</th>
                    <th>City</th>
                    <th>Update Status</th>
                </tr>
                </thead>
                <tbody>
                {weatherLogs.map((weatherLog, index) => (
                    <tr key={`${weatherLog.city}-${weatherLog.updateDateUtc}-${index}`}>
                        <td>{new Date(weatherLog.updateDateUtc).toLocaleString()}</td>
                        <td>{weatherLog.city}</td>
                        <td style={{
                            color: weatherLog.isUpdateSuccessful ? 'green' : 'red'
                        }}>
                            {weatherLog.isUpdateSuccessful ? 'Success' : 'Failed'}
                        </td>
                    </tr>
                ))}
                </tbody>
            </table>
    );
};

export default WeatherLogList;