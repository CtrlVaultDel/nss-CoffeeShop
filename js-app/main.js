const beanUrl = "https://localhost:5001/api/beanvariety/";
const coffeeUrl = "https://localhost:5001/api/coffee/";

// Document Targets
const beanTarget = document.querySelector('.bean_container');
const formTarget = document.querySelector('.form_container');
const coffeeTarget = document.querySelector('.coffee_container');

// Add an event listener for the initial button
const runButton = document.querySelector("#run-button");
runButton.addEventListener("click", () => {
    getAllBeanVarieties()
        .then(beanVarieties => {
            beanTarget.innerHTML = listBeanVarieties(beanVarieties);
        })
});

// Add an event listener onto the button that will allow the user to add a new coffee bean
const addBeanButton = document.querySelector("#add-bean-button");
addBeanButton.addEventListener("click", () => {
    formTarget.innerHTML = beanInputForm();
})

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

const beanInputForm = () => {
    return `
    <form>
        <fieldset>
            <div>
                <label for="name">Bean Name: <abbr title="required" aria-label="required">*</abbr></label>
                <input id="name" type="text" name="name">
            </div>
        </fieldset>
        <fieldset>
            <div>
                <label for="region">Bean Region: <abbr title="required" aria-label="required">*</abbr></label>
                <input id="region" type="text" name="region">
            </div>
        </fieldset>
        <fieldset>
            <div>
                <label for="notes">Bean Notes: </label>
                <input id="notes" type="textbox" name="notes">
            </div>
        </fieldset>
        <button type ="submit">Add Bean</button>
    </form>
    `
}