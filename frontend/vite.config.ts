import { defineConfig } from 'vite'
import react, { reactCompilerPreset } from '@vitejs/plugin-react'
import babel from '@rolldown/plugin-babel'

// https://vite.dev/config/
export default defineConfig({
  plugins: [
    react(),
    babel({ presets: [reactCompilerPreset()] })
  ],
  server: {
        proxy: {
            '/api': {
                target: 'http://localhost:5188',
                rewrite: (path) => path.replace(/^\/api/, ''),
                changeOrigin: true,
                secure: false
            }
        }
    },

})
