import React, { useState, useEffect } from 'react';
import {WeatherData} from "../types/WeatherData";
import {Bar, BarChart, CartesianGrid, Legend, Rectangle, ResponsiveContainer, Tooltip, XAxis, YAxis} from "recharts";
import {WeatherTooltip} from "../components/WeatherTooltip";

// Interface for API response
interface ApiWeatherData {
    city: string;
    country: string;
    temperature: string;
    minTemperature: string;
    maxTemperature: string;
    lastUpdateTime: string;
}

const WeatherChart = () => {
    const [data, setData] = useState<WeatherData[]>([]);
    const [loading, setLoading] = useState<boolean>(false);
    const [error, setError] = useState<string | null>(null);
    const [selectedDateTime, setSelectedDateTime] = useState<string>('');
    const [hasData, setHasData] = useState<boolean>(false);
    const [isInitialLoad, setIsInitialLoad] = useState<boolean>(true);

    // Function to convert comma decimal separator to dot
    const parseTemperature = (tempString: string): number => {
        return parseFloat(tempString.replace(',', '.'));
    };

    // Function to format datetime for API call
    const formatDateTimeForAPI = (dateTimeString: string): string => {
        if (!dateTimeString) return '';
        const date = new Date(dateTimeString);
        return date.toISOString();
    };

    // Function to fetch weather data from API
    const fetchWeatherData = async (dateTime?: string) => {
        try {
            setLoading(true);
            setError(null);

            // Build URL with optional datetime parameter
            let url = 'http://localhost:5218/api/weather/updates';
            if (dateTime) {
                const formattedDateTime = formatDateTimeForAPI(dateTime);
                url += `?date=${encodeURIComponent(formattedDateTime)}`;
            }

            const response = await fetch(url);

            if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`);
            }

            const apiData: ApiWeatherData[] = await response.json();

            // Transform API data to match WeatherData interface
            const transformedData: WeatherData[] = apiData.map(item => ({
                city: item.city,
                country: item.country,
                temperature: parseTemperature(item.temperature),
                minTemperature: parseTemperature(item.minTemperature),
                maxTemperature: parseTemperature(item.maxTemperature),
                lastUpdateTime: item.lastUpdateTime
            }));

            setData(transformedData);
            setHasData(true);
            setIsInitialLoad(false);

        } catch (err) {
            console.error('Error fetching weather data:', err);
            setError(err instanceof Error ? err.message : 'An error occurred');
            setData([]);
            setHasData(false);
            setIsInitialLoad(false);
        } finally {
            setLoading(false);
        }
    };

    // Auto-fetch data when component mounts
    useEffect(() => {
        fetchWeatherData(); // Fetch latest data without date filter
    }, []);

    // Handle datetime selection
    const handleDateTimeChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        const newDateTime = event.target.value;
        setSelectedDateTime(newDateTime);

        // Clear previous data when datetime changes
        setData([]);
        setHasData(false);
        setError(null);
    };

    // Handle fetch data button click
    const handleFetchData = () => {
        if (selectedDateTime) {
            fetchWeatherData(selectedDateTime);
        }
    };

    // Get current datetime in YYYY-MM-DDTHH:MM format for max attribute
    const getCurrentDateTime = (): string => {
        const now = new Date();
        const year = now.getFullYear();
        const month = String(now.getMonth() + 1).padStart(2, '0');
        const day = String(now.getDate()).padStart(2, '0');
        const hours = String(now.getHours()).padStart(2, '0');
        const minutes = String(now.getMinutes()).padStart(2, '0');
        return `${year}-${month}-${day}T${hours}:${minutes}`;
    };

    // Get datetime 30 days ago for min attribute
    const getMinDateTime = (): string => {
        const date = new Date();
        date.setDate(date.getDate() - 30);
        const year = date.getFullYear();
        const month = String(date.getMonth() + 1).padStart(2, '0');
        const day = String(date.getDate()).padStart(2, '0');
        return `${year}-${month}-${day}T00:00`;
    };

    return (
        <div style={{ padding: '20px' }}>
            <h1>Weather Graph</h1>

            {/* Date Picker Section */}
            <div style={{
                marginBottom: '30px',
                padding: '20px',
                border: '1px solid #ddd',
                borderRadius: '8px',
                backgroundColor: '#f9f9f9'
            }}>
                <h3 style={{ marginTop: 0 }}>Select Date & Time for Historical Weather Data</h3>
                <p style={{ color: '#666', fontSize: '14px', marginTop: 0 }}>
                    Leave blank to show current/latest data, or select a specific date and time for historical data.
                </p>
                <div style={{ display: 'flex', alignItems: 'center', gap: '15px', flexWrap: 'wrap' }}>
                    <div>
                        <label htmlFor="datetime-picker" style={{ display: 'block', marginBottom: '5px', fontWeight: 'bold' }}>
                            Date & Time (Optional):
                        </label>
                        <input
                            id="datetime-picker"
                            type="datetime-local"
                            value={selectedDateTime}
                            onChange={handleDateTimeChange}
                            min={getMinDateTime()}
                            max={getCurrentDateTime()}
                            style={{
                                padding: '8px 12px',
                                border: '1px solid #ccc',
                                borderRadius: '4px',
                                fontSize: '14px',
                                minWidth: '200px'
                            }}
                        />
                    </div>

                    <button
                        onClick={handleFetchData}
                        disabled={!selectedDateTime || loading}
                        style={{
                            padding: '10px 20px',
                            backgroundColor: selectedDateTime && !loading ? '#007bff' : '#ccc',
                            color: 'white',
                            border: 'none',
                            borderRadius: '4px',
                            cursor: selectedDateTime && !loading ? 'pointer' : 'not-allowed',
                            fontSize: '14px',
                            marginTop: '20px'
                        }}
                    >
                        {loading ? 'Fetching...' : 'Fetch Historical Data'}
                    </button>
                </div>

                {selectedDateTime && (
                    <p style={{ marginTop: '10px', color: '#666', fontSize: '14px' }}>
                        Selected: {new Date(selectedDateTime).toLocaleString('en-US', {
                        weekday: 'long',
                        year: 'numeric',
                        month: 'long',
                        day: 'numeric',
                        hour: '2-digit',
                        minute: '2-digit',
                        hour12: true
                    })}
                    </p>
                )}
            </div>

            {/* Loading State */}
            {loading && (
                <div style={{
                    textAlign: 'center',
                    padding: '40px 20px',
                    color: '#666',
                    backgroundColor: '#f0f8ff',
                    borderRadius: '8px',
                    border: '1px solid #cce7ff'
                }}>
                    <h3 style={{ color: '#0066cc', marginBottom: '10px' }}>Loading Weather Data...</h3>
                    <p>Please wait while we fetch the latest information.</p>
                </div>
            )}

            {/* Error State */}
            {error && (
                <div style={{
                    color: '#d32f2f',
                    marginBottom: '20px',
                    padding: '15px',
                    backgroundColor: '#ffebee',
                    borderRadius: '4px',
                    border: '1px solid #ffcdd2'
                }}>
                    <strong>Error:</strong> {error}
                    <div style={{ marginTop: '5px', fontSize: '14px' }}>
                        Please try selecting a different date/time or check your connection.
                    </div>
                </div>
            )}

            {/* Chart Display */}
            {hasData && data.length > 0 && !loading && (
                <div>
                    <div style={{ marginBottom: '15px', textAlign: 'center' }}>
                        <h3 style={{ color: '#333', margin: 0 }}>
                            {selectedDateTime
                                ? `Weather Data for ${new Date(selectedDateTime).toLocaleString('en-US', {
                                    weekday: 'long',
                                    year: 'numeric',
                                    month: 'long',
                                    day: 'numeric',
                                    hour: '2-digit',
                                    minute: '2-digit',
                                    hour12: true
                                })}`
                                : 'Current Weather Data'
                            }
                        </h3>
                        <p style={{ color: '#666', margin: '5px 0 0 0', fontSize: '14px' }}>
                            Showing data for {data.length} cities
                        </p>
                    </div>

                    <ResponsiveContainer width="100%" height={400}>
                        <BarChart
                            data={data}
                            margin={{
                                top: 20,
                                right: 30,
                                left: 20,
                                bottom: 5,
                            }}
                        >
                            <CartesianGrid strokeDasharray="3 3" />
                            <XAxis dataKey="city" />
                            <YAxis label={{ value: 'Temperature (°C)', angle: -90, position: 'insideLeft' }} />
                            <Tooltip content={<WeatherTooltip />}/>
                            <Legend />
                            <Bar dataKey="minTemperature" name="Minimum Temperature" fill="#64b5f6" activeBar={<Rectangle fill="#42a5f5" stroke="#1976d2" />} />
                            <Bar dataKey="temperature" name="Current Temperature" fill="#ffb74d" activeBar={<Rectangle fill="#ffa726" stroke="#f57c00" />} />
                            <Bar dataKey="maxTemperature" name="Maximum Temperature" fill="#81c784" activeBar={<Rectangle fill="#66bb6a" stroke="#388e3c" />} />
                        </BarChart>
                    </ResponsiveContainer>
                </div>
            )}

            {/* No Data State - only show if we're not loading and have no data after initial load */}
            {!hasData && !loading && !isInitialLoad && (
                <div style={{
                    textAlign: 'center',
                    padding: '40px 20px',
                    color: '#666',
                    backgroundColor: '#fff3cd',
                    borderRadius: '8px',
                    border: '1px solid #ffeaa7'
                }}>
                    <h3 style={{ color: '#856404', marginBottom: '10px' }}>No Data Available</h3>
                    <p>No weather data was found for the selected criteria.</p>
                </div>
            )}
        </div>
    );
}

export default WeatherChart;