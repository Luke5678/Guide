/** @type {import('tailwindcss').Config} */
module.exports = {
    content: ['./src/**/*.{razor,html}'],
    safelist: [
        {
            pattern: /bg-+/,
            variants: ['hover', 'focus'],
        }
    ],
    theme: {
        extend: {},
    },
    plugins: [],
}

