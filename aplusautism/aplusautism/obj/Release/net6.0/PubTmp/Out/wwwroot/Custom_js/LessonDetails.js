
debugger;
const fileUpload = document.getElementById("FileUpload1");

fileUpload.addEventListener("change", event => {


    debugger;

    //const file = event.target.files[0];
    //const videoEl = document.createElement("video");
    //videoEl.src = window.URL.createObjectURL(file);

    //// When the video metadata has loaded, check
    //// the video width/height
    //videoEl.onloadedmetadata = event => {
    //    window.URL.revokeObjectURL(videoEl.src);
    //    const { name, type } = file;
    //    const { videoWidth, videoHeight } = videoEl;



    //    // If there's an error, most likely because the file
    //    // is not a video, display an error.
    //    videoEl.onerror = () => {


    //      alert("hey")".mp4", ".webm", ".ogg"
    //    }
    //}

    debugger;
    var ext = $('#FileUpload1').val().split('.').pop().toLowerCase();
    if ($.inArray(ext, ['mp4', 'webm', 'ogg']) == -1) {
        //alert('invalid extension!');
        swal("Oops!", " Invalid File Type  ")
        $('#FileUpload1').val('');
        return false;
    }

      



})

const fileUploadtrial = document.getElementById("FileUpload2");

fileUploadtrial.addEventListener("change", event => {
    var ext = $('#FileUpload2').val().split('.').pop().toLowerCase();
    if ($.inArray(ext, ['mp4', 'webm', 'ogg']) == -1) {
        //alert('invalid extension!');
        swal("Oops!", " Invalid File Type ")
        $('#FileUpload1').val('');
        return false;
    }





})