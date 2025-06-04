import { createApp } from 'vue'
import './style.css'
import VueButton from './components/VueButton.vue';
import DestinationCard from './components/DestinationCard.vue';
import Article from "./components/Article.vue";
import FavoriteButton from "./components/FavoriteButton.vue";

console.log('Hello World')
const app = createApp({});
  app.component('FfVueButton', VueButton);
  app.component('FfDestinationCard', DestinationCard)
  app.component('FfArticle', Article);
  app.component('FfFavoriteButton', FavoriteButton);
  app.mount('#app');
