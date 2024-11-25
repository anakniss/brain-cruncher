import { createApp } from 'vue';
import { createPinia } from 'pinia';
import App from '../src/App.vue';
import router from '../src/router';
import './style.css';

//using mock auth to test
import { useAuthStore } from './stores/mockAuth';

const app = createApp(App);
const pinia = createPinia();

app.use(pinia);
app.use(router);


const authStore = useAuthStore();

app.mount('#app');