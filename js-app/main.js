const url = "https://localhost:5001/api/beanvariety/";
const target = document.querySelector('.bean_container');

const runButton = document.querySelector("#run-button");
runButton.addEventListener("click", () => {
    getAllBeanVarieties()
        .then(beanVarieties => {
            target.innerHTML = listBeanVarieties(beanVarieties);
        })
});

const addBeanButton = document.querySelector("#add-bean-button");
addBeanButton.addEventListener("click", () => {
    
})

// Gets all Bean Varieties from the server
const getAllBeanVarieties = () => fetch(url).then(resp => resp.json());

// Separates the array of bean varieties and return a list of HTML
const listBeanVarieties = (beanVarieties) => {
    var beanString = "";
    beanVarieties.forEach(bv => {
        beanString += `
        <div class="bean">
            <div class="bean_name"><b>Name:</b> ${bv.name}</div>
            <div class="bean_region"><b>Region:</b> ${bv.region}</div>
            <div class="bean_note"><b>Note:</b> ${verifyNote(bv.notes)}</div>
        </div>
        `
    })
    return beanString;
}

const verifyNote = (note) => {
    if(note !== null){
        return note;
    }
    return "N/A";
}