import {WeatherData} from "../types/WeatherData";
import {Bar, BarChart, CartesianGrid, Legend, Rectangle, ResponsiveContainer, Tooltip, XAxis, YAxis} from "recharts";
import {WeatherTooltip} from "../components/WeatherTooltip";

const WeatherChart = () => {
    const data : WeatherData[] = [
        {minTemperature: 10, maxTemperature: 20, temperature: 15, country: "USA", city: "New York", lastUpdateTime: "2024-03-20T12:00:00Z"},
        {minTemperature: 15, maxTemperature: 25, temperature: 20, country: "USA", city: "Los Angeles", lastUpdateTime: "2024-03-21T12:00:00Z"},
        {minTemperature: 5, maxTemperature: 15, temperature: 10, country: "USA", city: "Chicago", lastUpdateTime: "2024-03-22T12:00:00Z"}
    ]

  return (
    <div>
      <h1>Weather Graph</h1>
        <ResponsiveContainer width="80%" height={300}>
            <BarChart
                width={500}
                height={300}
                data={data}
                margin={{
                    top: 5,
                    right: 30,
                    left: 20,
                    bottom: 5,
                }}
            >
                <CartesianGrid strokeDasharray="3 3" />
                <XAxis dataKey="city" />
                <YAxis />
                <Tooltip content={<WeatherTooltip />}/>
                <Legend />
                <Bar dataKey="minTemperature" name="Minimum temperature" fill="#8884d8" activeBar={<Rectangle fill="pink" stroke="blue" />} />
                <Bar dataKey="maxTemperature" name="Maximum temperature" fill="#82ca9d" activeBar={<Rectangle fill="gold" stroke="purple" />} />
                <Bar dataKey="temperature" name="Temperature" fill="#82ca9d" activeBar={<Rectangle fill="gold" stroke="purple" />} />
            </BarChart>
        </ResponsiveContainer>
    </div>
  );
}

export default WeatherChart;