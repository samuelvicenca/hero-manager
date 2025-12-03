import { createRouter, createWebHistory } from 'vue-router';
import type { RouteRecordRaw } from 'vue-router';

import SuperpowerFormView from '../views/SuperpowerFormView.vue';
import HeroesListView from '../views/HeroesListView.vue';
import HeroFormView from '../views/HeroFormView.vue';

const routes: RouteRecordRaw[] = [
  {
    path: '/',
    name: 'HeroesList',
    component: HeroesListView,
  },
  {
    path: '/heroes',
    name: 'HeroesListAlias',
    component: HeroesListView,
  },
  {
    path: '/heroes/new',
    name: 'HeroCreate',
    component: HeroFormView,
  },
  {
    path: '/heroes/:id',
    name: 'HeroEdit',
    component: HeroFormView,
    props: true,
  },
  {
    path: '/superpowers/new',
    name: 'SuperpowerCreate',
    component: SuperpowerFormView,
  },
];

const router = createRouter({
  history: createWebHistory('/'),
  routes,
});

export default router;
