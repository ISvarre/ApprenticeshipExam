import { defineConfig } from 'vite';
import tailwindcss from '@tailwindcss/vite'
import vue from '@vitejs/plugin-vue';
import { resolve } from 'path';

// https://vitejs.dev/config/
export default defineConfig({
  publicDir: './public/',
  base: './',
  resolve: {
    alias: {
      vue: 'vue/dist/vue.esm-bundler.js',
      '@': resolve(__dirname),
      '@assets': resolve(__dirname, './src/assets/'),
    },
  },
  build: {
    outDir: '../wwwroot/',
    manifest: true,
    sourcemap: false,
    minify: false,
    emptyOutDir: true,
    rollupOptions: {
      output: {
        chunkFileNames: 'js/[name].js',
        entryFileNames: 'js/[name].js',
        assetFileNames: ({ name }) => {
          if (/\.(gif|jpe?g|png|svg|ico)$/.test(name ?? '')) {
            return 'images/[name][extname]';
          }
          if (/\.css$/.test(name ?? '')) {
            return 'css/[name][extname]';
          }
          return '[name][extname]';
        },
      },
    },
  },
  runtimeCompiler: true,
  plugins: [
    tailwindcss(),

    vue({
      template: {
        compilerOptions: {
          isCustomElement: (tag) => tag.startsWith('ff-'),
        },
      },
    }),
  ],
});
