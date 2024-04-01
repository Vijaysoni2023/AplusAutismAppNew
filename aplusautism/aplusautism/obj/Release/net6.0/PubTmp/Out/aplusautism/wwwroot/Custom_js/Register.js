
//$("#Email").blur(function () {
//    debugger;
//    var emailValue = $(this).val(); // email value is taken by it
//    CheckIfUserExist(emailValue);
//});

//$("#Email").keyup(function () {
//    alert("Keup");
//});


// to check the value of email alreday existed or not
function CheckIfUserExist(EmailValue) {
    var id = EmailValue;
    $.ajax({
        type: "POST",
        url: "/Auth/Userexist/isuserexist?" + $.param({ isuserexist: EmailValue }),
        data: id,
        success: function (result) {
            if (result === "yes") {
                swal("Sorry!", "This User is Already Exist ")         
                $('#Email').val('');
            }  
        }
    });
}


//State details by country id  

$("#Country").change(function () {

    var CountryId = parseInt($(this).val());

    if (!isNaN(CountryId)) {
        var ddlState = $('#State');
        ddlState.empty();
        ddlState.append($("<option></option>").val('').html('Please wait ...'));
        $.ajax({
            url: '/Auth/GetStatelist',
            type: 'GET',
            dataType: 'json',
            data: { CountryId: CountryId },
            success: function (d) {

                ddlState.empty(); // Clear the please wait  
                ddlState.append($("<option></option>").val('').html('Select State'));
                $.each(d, function (i, states) {
                    ddlState.append($("<option></option>").val(states.stateId).html(states.name));
                });
            },
            error: function () {
                //alert('Error!');
            }
        });
    }


}); 

//City Bind By satate id  
$("#State").change(function () {
    debugger;
    var StateId = parseInt($(this).val());
    if (!isNaN(StateId)) {
        var ddlCity = $('#City');
        ddlCity.append($("<option></option>").val('').html('Please wait ...'));

        debugger;
        $.ajax({
            url: '/Auth/getcity',
            type: 'GET',
            dataType: 'json',
            data: { stateid: StateId },
            success: function (d) {
                ddlCity.empty(); // Clear the plese wait  
                ddlCity.append($("<option></option>").val('').html('Select City Name'));
                $.each(d, function (i, cities) {
                    ddlCity.append($("<option></option>").val(cities.id).html(cities.name));
                });
            },
            error: function () {
               // alert('Error!');
            }
        });
    }


});  
