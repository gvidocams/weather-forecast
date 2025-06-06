import {WeatherLog} from "../types/WeatherLog";


const WeatherLogList = () => {
    const weatherLogs: WeatherLog[] = [
        { retrievalDate: "2024-03-20", city: "New York", retrievalStatus: "Success" },
        { retrievalDate: "2024-03-21", city: "Los Angeles", retrievalStatus: "Success" },
        { retrievalDate: "2024-03-22", city: "Chicago", retrievalStatus: "Failed" },
        { retrievalDate: "2024-03-23", city: "Houston", retrievalStatus: "Success" },
        { retrievalDate: "2024-03-24", city: "Phoenix", retrievalStatus: "Success" }
    ]

    return (
        <table>
            <thead>
            <tr>
                <th>Retrieval date</th>
                <th>City</th>
                <th>Retrieval status</th>
            </tr>
            </thead>
            <tbody>
            {weatherLogs.map(weatherLog => (
                <tr>
                    <td>{weatherLog.retrievalDate}</td>
                    <td>{weatherLog.city}</td>
                    <td>{weatherLog.retrievalStatus}</td>
                </tr>
            ))}
            </tbody>
        </table>
    )
};

export default WeatherLogList;