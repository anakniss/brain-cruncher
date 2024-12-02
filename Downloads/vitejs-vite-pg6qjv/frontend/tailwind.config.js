/** @type {import('tailwindcss').Config} */
export default {
  content: [
    "./index.html",
    "./src/**/*.{vue,js,ts,jsx,tsx}",
  ],
  theme: {
    extend: {
      colors: {
        'custom-bg': '#FFFFFF',
        'custom-header': '#716377',
        'custom-box': '#ded4e2',
      },
      fontFamily: {
        fredoka: ['Fredoka', 'sans-serif'],
        spray: ['Rubik Spray Paint', 'cursive'],
      },
    },
  },
  plugins: [],
}
