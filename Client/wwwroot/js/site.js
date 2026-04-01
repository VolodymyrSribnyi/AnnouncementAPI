const element = document.getElementById("categoryData");
const map = JSON.parse(element.dataset.map);

const categoryFilter = document.getElementById("categoryFilter");
const subCategoryFilter = document.getElementById("subCategoryFilter");

const cards = document.querySelectorAll(".announcement-card");

function filterAnnouncements() {
    const selectedCategory = categoryFilter.value;
    const selectedSubCategory = subCategoryFilter.value;

    cards.forEach(card => {
        const cardCategory = card.dataset.category;
        const cardSubCategory = card.dataset.subcategory;

        const matchCategory = !selectedCategory || cardCategory === selectedCategory;
        const matchSubCategory = !selectedSubCategory || cardSubCategory === selectedSubCategory;

        if (matchCategory && matchSubCategory) {
            card.style.display = "block";
        } else {
            card.style.display = "none";
        }
    });
}

categoryFilter.addEventListener("change", filterAnnouncements);
subCategoryFilter.addEventListener("change", filterAnnouncements);



function includeSubCategory() {
    const selectedCategory = categoryFilter.value;

    console.log(selectedCategory);
    console.log(map[selectedCategory]);


    const subCategories = map[selectedCategory] || [];

    subCategoryFilter.options.length = 0;
    var opt = document.createElement('option');
    opt.value = "";
    opt.textContent = "Всі підкатегорії";
    subCategoryFilter.appendChild(opt);

    for (let i = 0; i < subCategories.length; i++) {
        var opt = document.createElement('option');
        opt.value = subCategories[i];
        opt.innerHTML = subCategories[i]; 
        subCategoryFilter.appendChild(opt);
    }
    console.log(subCategoryFilter);
}

categoryFilter.addEventListener("change", includeSubCategory);
//subCategoryFilter.addEventListener("change", includeSubCategory);
