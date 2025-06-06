import React from 'react';
import logo from './logo.svg';
import './App.css';
import { BrowserRouter, Routes, Route } from 'react-router-dom';
import WeatherLogs from "./pages/WeatherLogs";
import WeatherChart from "./pages/WeatherChart";

function App() {
  return (
      <BrowserRouter>
        <Routes>
          <Route path="/" element={<WeatherChart />} />
          <Route path="/weather-logs" element={<WeatherLogs />} />
        </Routes>
      </BrowserRouter>
  );
}

export default App;
