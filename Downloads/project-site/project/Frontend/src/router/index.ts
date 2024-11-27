import { createRouter, createWebHistory } from 'vue-router';
import { useAuthStore } from '../stores/mockAuth';
import Home from '../views/Home.vue';
import Login from '../views/Login.vue';
import Profile from '../views/Profile.vue';
import CreateQuiz from '../views/CreateQuiz.vue';
import PlayQuiz from '../views/PlayQuiz.vue';
import Ranking from '../views/Ranking.vue';
import Reports from '../views/Reports.vue';
import { Role } from '../types';

const router = createRouter({
  history: createWebHistory(),
  routes: [
    {
      path: '/',
      name: 'home',
      component: Home
    },
    {
      path: '/login',
      name: 'login',
      component: Login
    },
    {
      path: '/profile',
      name: 'profile',
      component: Profile,
      meta: { requiresAuth: true }
    },
    {
      path: '/create-quiz',
      name: 'create-quiz',
      component: CreateQuiz,
      meta: { requiresAuth: true, roles: [Role.Professor] }
    },
    {
      path: '/play',
      name: 'play-quiz',
      component: PlayQuiz,
      meta: { requiresAuth: true, roles: [Role.Student] }
    },
    {
      path: '/ranking',
      name: 'ranking',
      component: Ranking,
      meta: { requiresAuth: true }
    },
    {
      path: '/reports',
      name: 'reports',
      component: Reports,
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
  } else if (requiredRoles && authStore.currentUser) {
    if (!requiredRoles.includes(authStore.currentUser.role)) {
      next('/');
    } else {
      next();
    }
  } else {
    next();
  }
});

export default router;