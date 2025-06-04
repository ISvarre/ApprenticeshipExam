<script setup lang="ts">
import { ref } from "vue";

const isFavorite = ref(false);

const checkIfFavorite = () => {
  const currentPage = window.location.href;
  let favorites = JSON.parse(localStorage.getItem("favorites") || "[]");
  isFavorite.value = favorites.includes(currentPage);
};

const toggleFavorite = () => {
  const currentPage = window.location.href;
  let favorites = JSON.parse(localStorage.getItem("favorites") || "[]");

  if (favorites.includes(currentPage)) {
    favorites = favorites.filter((page :any) => page !== currentPage);
    isFavorite.value = false;
  } else {
    favorites.push(currentPage);
    isFavorite.value = true;
  }

  localStorage.setItem("favorites", JSON.stringify(favorites));
};

checkIfFavorite();
</script>

<template>
  <button @click="toggleFavorite" :class="{'bg-blue-500': isFavorite, 'bg-gray-500': !isFavorite}" class="p-2 rounded text-white">
    {{ isFavorite ? 'Remove from Favorites' : 'Save as Favorite' }}
  </button>
</template>
