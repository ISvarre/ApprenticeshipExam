<script setup lang="ts">
import { Utensils, Plus, CreditCard, X } from "lucide-vue-next";
import { ref, computed, watch, onMounted } from 'vue';

const props = defineProps({
    title: {
        type: String,
        default: 'Velkommen til lunsjassistenten! 🍽️'
    },
    description: {
        type: String,
        default: 'Her kan you enkelt koordinere lunsjen i lokalene våre. Registrer deg for dagens lunsj, legg til ønsker på innkjøpslisten, og hold oversikt over hvem som handler og hvor Kiwikortet er.'
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

const selectedLunchOptionId = ref<number | null>(null);

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

const canteenMenu = ref([
    {
        day: 'Mandag',
        meal: 'Taco',
        description: 'Med kjøtt, salat og tilbehør',
        highlighted: false,
    },
    {
        day: 'Tirsdag',
        meal: 'Indrefilet',
        description: 'Med potetmos og grønnsaker',
        highlighted: false,
    },
    {
        day: 'Onsdag',
        meal: 'Kylling Tikka Masala',
        description: 'Med basmatiris og nanbrød',
        highlighted: true,
    },
    {
        day: 'Torsdag',
        meal: 'Fiskegrateng',
        description: 'Med salat og brød',
        highlighted: false,
    },
    {
        day: 'Fredag',
        meal: 'Pizza',
        description: 'Dagens pizza med salat',
        highlighted: false,
    },
]);

function getTodayName(): string {
    const days = ['Søndag', 'Mandag', 'Tirsdag', 'Onsdag', 'Torsdag', 'Fredag', 'Lørdag'];
    const today = new Date().getDay();
    return days[today];
}
function updateMenuForToday() {
    const todayName = getTodayName();
    const menu = canteenMenu.value;
    menu.forEach(item => {
        const originalDayName = item.day.replace('I dag: ', '');

        if (originalDayName === todayName || item.day.startsWith('I dag')) {
            item.highlighted = true;
            item.day = 'I dag';
        } else {
            const originalEntry = canteenMenu.value.find(
                (original) => original.meal === item.meal && original.description === item.description
            );
            if (originalEntry && item.day !== originalEntry.day) {
                item.day = originalEntry.day;
            }
            item.highlighted = false;
        }
    });

    const todayIndex = menu.findIndex(item => item.day === 'I dag');
    if (todayIndex !== -1 && todayIndex > 0) {
        const [todayItem] = menu.splice(todayIndex, 1);
        menu.unshift(todayItem);
    }
}


onMounted(() => {
    updateMenuForToday();
});

const isEditingMenu = ref(false);
const editableMenu = ref([] as typeof canteenMenu.value);

function startEditMenu() {
    editableMenu.value = canteenMenu.value.map(item => ({ ...item }));
    isEditingMenu.value = true;
}

function saveMenu() {
    canteenMenu.value = editableMenu.value.map(item => ({ ...item }));
    isEditingMenu.value = false;
}

function cancelEditMenu() {
    isEditingMenu.value = false;
}

const lunchSummary = computed(() => {
    const summary: { name: string; people: number }[] = [];
    const otherSuggestions: { name: string; people: number }[] = [];

    // Filter out "Kantina" and "Lokalet" and group others
    lunchOptions.value.forEach(option => {
        if (option.name === 'Kantina' || option.name === 'Lokalet') {
            summary.push({ name: option.name, people: option.people });
        } else {
            otherSuggestions.push({ name: option.name, people: option.people });
        }
    });

    return {
        mainOptions: summary,
        otherSuggestions: otherSuggestions,
    };
});

const kiwiCardStatus = ref(true);
</script>

<template>
    <div class="w-full min-h-screen bg-[#e8efff] py-8">
        <div class="flex flex-col max-w-7xl mx-auto gap-8 px-4 sm:px-6 lg:px-8">
            <!-- Welcome Section -->
            <div class="flex flex-col w-full gap-2 p-6 bg-white rounded-md drop-shadow-xs">
                <h1 class="text-2xl font-semibold">{{ props.title }}</h1>
                <p class="text-sm text-gray-500">{{ props.description }}</p>
            </div>

            <div class="flex w-full gap-6">
                <div class="flex flex-col h-fit bg-white p-6 rounded-md shadow-sm w-1/2">
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
                                ' border rounded-lg p-5 shadow-sm w-fit cursor-pointer transition-all duration-200 ease-in-out hover:shadow-lg hover:-translate-y-0.5',
                                option.selected
                                  ? 'border-blue-500 ring-2 ring-blue-200 bg-blue-50'
                                  : 'border-gray-200 bg-white',
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
                <div class="flex flex-col bg-white p-6 rounded-md w-1/4">
                    <div class="mb-6">
                        <div class="flex items-center gap-2 mb-1">
                            <Utensils class="w-5 h-5 text-gray-800" />
                            <h1 class="text-2xl font-semibold text-gray-800">
                                Kantinemeny
                            </h1>
                        </div>
                        <p class="text-gray-500 text-sm">
                            Se hva som serveres i kantina
                        </p>
                    </div>

                    <div class="flex flex-col gap-4">
                        <div
                            v-for="(item, idx) in isEditingMenu ? editableMenu : canteenMenu"
                            :key="isEditingMenu ? idx : item.day"
                            :class="[
                                'rounded-lg p-4 flex flex-col gap-1.5 shadow-sm',
                                item.highlighted
                                    ? 'bg-blue-50 border border-blue-500'
                                    : 'bg-white border border-gray-200',
                            ]"
                        >
                            <template v-if="isEditingMenu">
                                <div class="mb-1 p-1 font-semibold text-gray-700">
                                    {{ item.day }}
                                </div>
                                <input
                                    v-model="item.meal"
                                    class="mb-1 p-1 border rounded"
                                    placeholder="Måltid"
                                />
                                <input
                                    v-model="item.description"
                                    class="p-1 border rounded"
                                    placeholder="Beskrivelse"
                                />
                            </template>
                            <template v-else>
                                <h3 class="font-semibold text-md leading-tight">
                                    <span class="whitespace-nowrap text-blue-700">
                                        {{ item.day }}:
                                    </span>
                                    <span
                                        class="text-gray-900"
                                    >
                                        {{ item.meal }}
                                    </span>
                                </h3>
                                <p class="text-gray-700 text-sm">
                                    {{ item.description }}
                                </p>
                            </template>
                        </div>
                    </div>

                    <button
                        v-if="!isEditingMenu"
                        class="mt-6 w-full py-3 px-4 bg-white border border-gray-300 rounded-md text-gray-800 font-medium hover:bg-gray-50 transition-colors duration-200"
                        @click="startEditMenu"
                    >
                        Oppdater meny
                    </button>
                    <div v-else class="flex gap-2 mt-6">
                        <button
                            class="py-2 px-4 bg-blue-600 text-white rounded hover:bg-blue-700"
                            @click="saveMenu"
                        >
                            Lagre
                        </button>
                        <button
                            class="py-2 px-4 bg-gray-200 text-gray-800 rounded hover:bg-gray-300"
                            @click="cancelEditMenu"
                        >
                            Avbryt
                        </button>
                    </div>
                </div>

                <div class="flex flex-col h-fit w-1/4 gap-6">
                    <div class="flex flex-col items-center h-fit bg-white p-6 rounded-md shadow-sm w-full gap-3">
                        <div class="flex items-center justify-start gap-2 pb-2">
                            <CreditCard class="w-6 h-6 text-black" />
                            <h1 class="text-2xl font-semibold">Kiwikort Status </h1>
                        </div>
                        <div class="w-full flex justify-center">
                            <div class="flex items-center justify-center rounded-full h-16 w-16 bg-green-200">
                                <CreditCard class="w-8 h-8 text-green-700"></CreditCard>
                            </div>
                        </div>
                        <div class="flex justify-center items-center bg-green-700 rounded-full">
                            <p class="px-3 py-1 text-sm text-white">{{ kiwiCardStatus ? 'Tilgjengelig' : 'utilgjengelig' }}</p>
                        </div>
                        <p class="text-gray-700 text-sm">
                            Kortet ligger på sin faste plass
                        </p>
                        <button type="button" class="w-full border-gray-500 border rounded-md font-semibold text-gray-700 text-md py-1"> Jeg tar kortet </button>
                    </div>
                    <div class="flex flex-col items-center h-fit bg-white p-6 rounded-md shadow-sm w-full gap-3">
                        <div class="flex flex-col gap-1 border-b border-gray-400 pb-5">
                            <h1 class="text-2xl font-semibold">Hvem handler i dag?</h1>
                            <p class="text-sm text-gray-700 pb-5">
                                Regitrer deg som innkjøpsansvarlig
                            </p>
                            <div class="flex bg-blue-50 justify-between p-2 rounded-md items-center w-full">
                                <div class="flex flex-col">
                                    <h2 class="text-md font-semibold">
                                        Erik Hansen
                                    </h2>
                                    <p class="text-xs text-gray-700">Registrert 11:30 </p>
                                </div>
                                <X class="w-5 h-5 text-red-500" />
                            </div>
                        </div>
                        <button type="button" class="bg-black p-2 w-full font-semibold text-md rounded-md text-white items-center">
                            Jeg går på butikken
                        </button>

                    </div>
                    <div class="flex flex-col h-fit bg-white p-6 rounded-md shadow-sm w-full">
                        <h1 class="text-2xl font-semibold mb-6">Dagens oversikt</h1>

                        <div class="flex flex-col gap-4">
                            <div
                                v-for="option in lunchSummary.mainOptions"
                                :key="option.name"
                                class="flex justify-between items-center pb-2 border-b border-gray-300 last:border-b-0"
                            >
                                <span class="text-md text-gray-800">{{ option.name }}:</span>
                                <div
                                    class="inline-block bg-gray-100 text-gray-800 text-xs font-medium px-3 py-1 rounded-full"
                                >
                                    {{ option.people }} personer
                                </div>
                            </div>

                            <div v-if="lunchSummary.otherSuggestions.length > 0">
                                <h2 class="text-lg font-bold text-gray-700 mt-4 mb-3">
                                    Andre forslag:
                                </h2>
                                <div
                                    v-for="option in lunchSummary.otherSuggestions"
                                    :key="option.name"
                                    class="flex justify-between items-center pb-2 border-b border-gray-300 last:border-b-0"
                                >
                                    <span class="text-lg text-gray-800">{{ option.name }}:</span>
                                    <div
                                        class="inline-block bg-gray-100 text-gray-800 text-xs font-medium px-3 py-1 rounded-full"
                                    >
                                        {{ option.people }} personer
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<style scoped>
</style>
