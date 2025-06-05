<script setup lang="ts">
import { defineProps } from 'vue'

const props = defineProps({
  title: {
    type: String,
    default: 'Velkommen til lunsjassistenten! 🍽️'
  },
  description: {
    type: String,
    default: 'Her kan du enkelt koordinere lunsjen i lokalene våre. Registrer deg for dagens lunsj, legg til ønsker på innkjøpslisten, og hold oversikt over hvem som handler og hvor Kiwikortet er.'
  }
});
</script>

<template>
  <div class="w-full h-full bg-[#e8efff]">
    <div class="flex max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
      <div class="flex flex-col w-full gap-2 p-6 bg-white rounded-md drop-shadow-xs">
        <h1 class="text-2xl font-semibold">
            {{ props.title }}
        </h1>
          <p class="text-sm text-gray-500">
            {{ props.description }}
          </p>
      </div>
    </div>
  </div>
</template>

<style scoped>

</style>
