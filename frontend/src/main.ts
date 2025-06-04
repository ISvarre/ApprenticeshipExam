import { createApp } from 'vue'
import './style.css'
import VueButton from './components/VueButton.vue';
import Header from './components/Header.vue';

console.log('Hello World')
const app = createApp({});
  app.component('FfVueButton', VueButton);
  app.component('FfHeader', Header);
  app.mount('#app');
