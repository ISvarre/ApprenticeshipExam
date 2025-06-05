import { createApp } from 'vue'
import './style.css'
import Header from './components/Header.vue';
import frontPage from './components/frontPage.vue';

const app = createApp({});
  app.component('FfHeader', Header);
  app.component('FfFrontPage', frontPage);
  app.mount('#app');
