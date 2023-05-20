document.querySelectorAll(".drop-zone__input").forEach((inputElement) => {
    const dropZoneElement = inputElement.closest(".drop-zone");

    dropZoneElement.addEventListener("click", (e) => {
        inputElement.click();
    });

    inputElement.addEventListener("change", (e) => {
        //alert('ss')
        if (inputElement.files.length) {
            updateThumbnail(dropZoneElement, inputElement.files[0]);

            }
    });
    dropZoneElement.addEventListener("dragover", (e) => {
        e.preventDefault();
        dropZoneElement.classList.add("drop-zone--over");
    });

    ["dragleave", "dragend"].forEach((type) => {
        dropZoneElement.addEventListener(type, (e) => {
            dropZoneElement.classList.remove("drop-zone--over");
        });
    });
    dropZoneElement.addEventListener("drop", (e) => {
        //alert('ss')
        e.preventDefault();
        if (e.dataTransfer.files.length) {
            inputElement.files = e.dataTransfer.files;
            updateThumbnail(dropZoneElement, e.dataTransfer.files[0]);

            
            }
        //alert('ss')
        dropZoneElement.classList.remove("drop-zone--over");
    });
});

/**
    * Updates the thumbnail on a drop zone element.
    *
    * @@param {HTMLElement} dropZoneElement
    * @@param {File} file
    */
function updateThumbnail(dropZoneElement, file) {
    let thumbnailElement = dropZoneElement.querySelector(".drop-zone__thumb");
    //alert('s')
    $('.addedfile').parent().remove();
    $(".addedfiles").append('<div class="col-lg-12"><div class="col-lg-2"></div><div class="border border-dark addeddiv col-lg-8 bg-white drop-zone p-2"><span class="col-lg-8 addedfile">' + file.name + '</span><span> (حجم الملف:"' + bytesToSize(file.size) + ')</span><span class="fileeditedname" id="' + file.type + '"></span><a class="btn-sm btn-danger text-white deletefile rounded-0"><span class="fa fa-times"></span> حذف</a></div><div class="col-lg-2"></div></div>')

    // First time - remove the prompt
    //if (dropZoneElement.querySelector(".drop-zone__prompt")) {
    //    dropZoneElement.querySelector(".drop-zone__prompt").remove();
    //}

    // First time - there is no thumbnail element, so lets create it
    if (!thumbnailElement) {
        thumbnailElement = document.createElement("div");
        thumbnailElement.classList.add("drop-zone__thumb");
        dropZoneElement.appendChild(thumbnailElement);
        $('.addedfile').append(thumbnailElement)
    }

    thumbnailElement.dataset.label = file.name;

    // Show thumbnail for image files
    if (file.type.startsWith("image/")) {
        const reader = new FileReader();

        reader.readAsDataURL(file);
        reader.onload = () => {
            thumbnailElement.style.backgroundImage = `url('${reader.result}')`;
        };
    } else {
        thumbnailElement.style.backgroundImage = null;
    }
}