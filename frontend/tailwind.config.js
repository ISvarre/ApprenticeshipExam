/** @type {import('tailwindcss').Config} */
module.exports = {
  content: [
    "./src/**/*.{vue,js,ts,jsx,tsx}", // Include all Vue and JS/TS files
    "../Views/**/*.cshtml",
    "../Views/StartPage/Index.cshtml",
    "../**/*.cshtml"
  ],
  theme: {
    extend: {},
  },
  plugins: [],
};
