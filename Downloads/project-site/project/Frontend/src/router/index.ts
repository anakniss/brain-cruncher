import { createRouter, createWebHistory } from 'vue-router';
import { useAuthStore } from '../stores/mockAuth';  // mock auth for testing
import Home from '../views/Home.vue';
import Login from '../views/Login.vue';
import Profile from '../views/Profile.vue';
import CreateQuiz from '../views/CreateQuiz.vue';
import PlayQuiz from '../views/PlayQuiz.vue';
import Ranking from '../views/Ranking.vue';
import Reports from '../views/Reports.vue';
import type { Role } from '../types';

const router = createRouter({
  history: createWebHistory(),
  routes: [
    {
      path: '/',
      component: Home,
      name: 'home'
    },
    {
      path: '/login',
      component: Login,
      name: 'login'
    },
    {
      path: '/profile',
      component: Profile,
      name: 'profile',
      meta: { requiresAuth: true }
    },
    {
      path: '/create-quiz',
      component: CreateQuiz,
      name: 'create-quiz',
      meta: { requiresAuth: true, roles: ['professor'] }
    },
    {
      path: '/play',
      component: PlayQuiz,
      name: 'play-quiz',
      meta: { requiresAuth: true, roles: ['student'] }
    },
    {
      path: '/ranking',
      component: Ranking,
      name: 'ranking',
      meta: { requiresAuth: true }
    },
    {
      path: '/reports',
      component: Reports,
      name: 'reports',
      meta: { requiresAuth: true }
    }
  ]
});

router.beforeEach((to, from, next) => {
  const authStore = useAuthStore();
  const requiresAuth = to.matched.some(record => record.meta.requiresAuth);
  const requiredRoles = to.meta.roles as Role[] | undefined; 
  if (requiresAuth && !authStore.currentUser) {
    next('/login'); 
  } else if (requiredRoles && Array.isArray(requiredRoles)) {

    if (!requiredRoles.includes(authStore.currentUser?.role!)) {
      next('/');
    } else {
      next();
    }
  } else {
    next();
  }
});




export default router;