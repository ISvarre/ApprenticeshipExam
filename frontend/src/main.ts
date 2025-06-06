import { createApp } from 'vue'
import './style.css'
import Header from './components/Header.vue';
import frontPage from './components/frontPage.vue';
import ShoppingList from './components/shoppingList.vue';

const app = createApp({});
    app.component('FfHeader', Header);
    app.component('FfFrontPage', frontPage);
    app.component('FfShoppingList', ShoppingList);
    app.mount('#app');
