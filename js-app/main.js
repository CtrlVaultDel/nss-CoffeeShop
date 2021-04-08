const beanUrl = "https://localhost:5001/api/beanvariety/";
const coffeeUrl = "https://localhost:5001/api/coffee/";

// Document Targets
const saveBeanButton = document.getElementById('saveBean');
const beanTarget = document.querySelector('.bean_container');
const beanFormTarget = document.querySelector('.bean_form_container');
const coffeeTarget = document.querySelector('.coffee_container');

// Add an event listener for the initial button
const runButton = document.querySelector("#run-button");
runButton.addEventListener("click", () => {
    getAllBeanVarieties()
        .then(beanVarieties => {
            beanTarget.innerHTML = listBeanVarieties(beanVarieties);
        })
});

// Gets all Bean Varieties from the server
const getAllBeanVarieties = () => fetch(beanUrl).then(resp => resp.json());

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

// Verifies if a note is empty. Returns "N/A" if it is
const verifyNote = (note) => {
    if(note !== null){
        return note;
    }
    return "N/A";
}

// Saves new notes to the API
const saveBean = bean => {
    return fetch(beanUrl, {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify(bean)
    })
};

// Listen for a bean to be saved and attempt to post the new bean if it is clicked.
saveBeanButton.addEventListener("click", e => {
    // Save DOM elements to local variables
    let name = document.getElementById("bean--name").value;
    let region = document.getElementById("bean--region").value;
    let notes = document.getElementById("bean--notes").value;
    if(notes === ""){
        notes = null;
    }
    if (name.length > 3 && name.length < 50 && region.length > 3 && region.length && region.length < 255){
        // Make a new object representation of a bean
        const newBean = {
            name,
            region,
            notes
        };

        // Change API state and application state
        saveBean(newBean);

        // Clear the form
        beanFormTarget.clear();

        e.preventDefault();
    }
    else{
        alert("Please enter a 3-50 character name and 3-255 character region. Notes are optional.");
    }
})