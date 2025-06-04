<script setup lang="ts">
import { defineProps } from "vue";

const props = defineProps({
  image: String,
  title: String,
  location: String,
  tag: String,
  page: String
});
</script>

<template>
  <a
    :href="page"
    class="relative flex flex-col justify-end bg-cover bg-center rounded-lg shadow-lg overflow-hidden hover:shadow-xl transition-shadow duration-300 h-80 w-48 gap-2"
    :style="{ backgroundImage: `url(${image})` }"
  >
    <div
      class="absolute inset-0 bg-gradient-to-t from-black via-transparent to-transparent"
    ></div>
    <span
      class="absolute top-3 left-3 bg-red-500 text-white text-xs font-semibold py-1 px-3 rounded-full z-10"
    >
      {{ tag }}
    </span>
    <div class="relative p-4 text-center text-white">
      <h3 class="text-lg font-bold">
        {{ title }}
      </h3>
      <p class="text-sm">
        {{ location }}
      </p>
    </div>
  </a>
</template>
