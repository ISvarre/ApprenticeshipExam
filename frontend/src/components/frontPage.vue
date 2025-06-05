<script setup lang="ts">
import { Utensils, Plus } from "lucide-vue-next";
import { ref, computed } from 'vue';

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

const lunchOptions = ref([
    {
        id: 1,
        name: 'Kantina',
        description: 'Dagens varme måltider',
        people: 0,
        selected: false,
    },
    {
        id: 2,
        name: 'Lokalet',
        description: 'Påsmurt og delt måltid',
        people: 0,
        selected: false,
    },
]);

const showCreateForm = ref(false);

const newLunchOption = ref({
    name: '',
    description: '',
});

const selectedLunchOptionId = ref(2);

const selectedLunchOption = computed(() => {
    return lunchOptions.value.find(
        (option) => option.id === selectedLunchOptionId.value,
    );
});

const selectLunchOption = (id: number) => {
    const currentSelected = lunchOptions.value.find(option => option.selected);
    const clickedOption = lunchOptions.value.find(option => option.id === id);

    if (!clickedOption) return;

    if (clickedOption.selected) {
        clickedOption.selected = false;
        if (clickedOption.people > 0) clickedOption.people--;
        selectedLunchOptionId.value = null;
    } else {
        if (currentSelected) {
            currentSelected.selected = false;
            if (currentSelected.people > 0) currentSelected.people--;
        }
        clickedOption.selected = true;
        clickedOption.people++;
        selectedLunchOptionId.value = id;
    }
};

const addLunchOption = () => {
    if (newLunchOption.value.name.trim()) {
        const maxId = Math.max(...lunchOptions.value.map((o) => o.id));
        const newId = maxId > 0 ? maxId + 1 : 1;

        lunchOptions.value.push({
            id: newId,
            name: newLunchOption.value.name,
            description: newLunchOption.value.description,
            people: 0,
            selected: false,
        });

        newLunchOption.value = {
            name: '',
            description: '',
        };
        showCreateForm.value = false;
    } else {
        alert('Navn på sted er påkrevd!');
    }
};

const cancelCreate = () => {
    showCreateForm.value = false;
    newLunchOption.value = {
        name: '',
        description: '',
    };
};
</script>

<template>
    <div class="w-full h-full bg-[#e8efff] py-8">
        <div class="flex flex-col max-w-7xl mx-auto gap-8 px-4 sm:px-6 lg:px-8">
            <!-- Welcome Section -->
            <div class="flex flex-col w-full gap-2 p-6 bg-white rounded-md drop-shadow-xs">
                <h1 class="text-2xl font-semibold">{{ props.title }}</h1>
                <p class="text-sm text-gray-500">{{ props.description }}</p>
            </div>

            <div class="flex w-full">
                <div class="flex flex-col bg-white p-6 rounded-md shadow-sm w-1/2">
                    <div class="flex justify-between items-center mb-6">
                        <div>
                            <div class="flex gap-2 items-center shrink-0">
                                <Utensils class="w-5 h-5 text-gray-800" />
                                <h1 class="text-2xl font-semibold text-gray-800">
                                    Hvor spiser du lunsj i dag?
                                </h1>
                            </div>
                            <p class="text-sm text-gray-500">
                                Velg ditt foretrukne lunsjsted for i dag
                            </p>
                        </div>
                        <button
                            v-if="!showCreateForm"
                            @click="showCreateForm = true"
                            type="button"
                            class="justify-center gap-2 text-sm border border-gray-300 h-9 rounded-md px-3 flex items-center shrink-0 bg-white hover:bg-gray-50 transition-colors duration-200"
                        >
                            <Plus class="w-4 h-4 inline" />
                            Nytt forslag
                        </button>
                    </div>
                    <div
                        v-if="!showCreateForm"
                        class="w-full max-w-full flex flex-wrap gap-5"
                    >
                        <div
                            v-for="option in lunchOptions"
                            :key="option.id"
                            :class="[
                                'bg-white border rounded-lg p-5 shadow-sm w-fit cursor-pointer transition-all duration-200 ease-in-out hover:shadow-lg hover:-translate-y-0.5',
                                option.selected
                                  ? 'border-blue-500 ring-2 ring-blue-200'
                                  : 'border-gray-200',
                            ]"
                            @click="selectLunchOption(option.id)"
                        >
                            <h2 class="text-xl font-bold text-gray-900 mb-2">{{ option.name }}</h2>
                            <p class="text-sm text-gray-700 mb-4">{{ option.description }}</p>
                            <div
                                class="inline-block bg-gray-100 text-gray-800 text-xs font-medium px-3 py-1 rounded-full"
                            >
                                {{ option.people }} personer
                            </div>
                        </div>
                    </div>

                    <div
                        v-else
                        class="bg-white border border-gray-200 rounded-lg p-5 shadow-sm flex flex-col space-y-4"
                    >
                        <input
                            type="text"
                            v-model="newLunchOption.name"
                            placeholder="Navn på sted..."
                            class="p-3 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
                        />
                        <textarea
                            v-model="newLunchOption.description"
                            placeholder="Kort beskrivelse..."
                            rows="3"
                            class="p-3 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500 resize-y"
                        ></textarea>
                        <div class="flex justify-end space-x-3 mt-4">
                            <button
                                @click="addLunchOption"
                                class="bg-blue-600 text-white py-2 px-5 rounded-md text-base font-medium hover:bg-blue-700 focus:outline-none focus:ring-2 focus:ring-blue-500 focus:ring-offset-2"
                            >
                                Legg til forslag
                            </button>
                            <button
                                @click="cancelCreate"
                                class="bg-gray-200 text-gray-800 py-2 px-5 rounded-md text-base font-medium hover:bg-gray-300 focus:outline-none focus:ring-2 focus:ring-gray-400 focus:ring-offset-2"
                            >
                                Avbryt
                            </button>
                        </div>
                    </div>

                    <div
                        v-if="!showCreateForm && selectedLunchOption"
                        class="mt-6 bg-blue-50 border border-blue-200 text-blue-800 p-4 rounded-lg text-center"
                    >
                        Du har valgt:
                        <span class="font-semibold">{{ selectedLunchOption.name }}</span>
                        <p class="text-sm mt-1">
                            Klikk på et annet alternativ for å endre ditt valg
                        </p>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<style scoped>
</style>
