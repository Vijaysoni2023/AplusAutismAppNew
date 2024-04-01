


function EditPlan(SId) {
    var id = SId;
    $.ajax({
        type: "POST",
        url: "/Admin/CustSubscription/EditPlans?" + $.param({ subscriptionId: id }),
        data: id,
        success: function (result) {

            debugger;

            $('#originplan_' + SId).hide();

            $('#editplan_' + SId).show();

            $('#editMonth_' + SId).val(result.duration);
            $('#editPrice_' + SId).val(result.price);

            if (result.durationType == "M") {
                $('#Editplandurationlable_' + SId).text("Month");
            }
            if (result.durationType == "Y") {
                $('#Editplandurationlable_' + SId).text("Year");
            }

            $('#UpdateDivanbutton_' + SId).show();
            $('#originplanDivbutton_' + SId).hide();
        
        }
    });
}


function CancelPlan(sid) {
    debugger;

    $('#originplan_' + sid).show();

    $('#editplan_' + sid).hide();

    $('#UpdateDivanbutton_' + sid).hide();
    $('#originplanDivbutton_' + sid).show();

}


function UpdatePlan(sid) {
    debugger;




    var monthdatavalue = $('#editMonth_' + sid).val();
    var pricedatavalue = $('#editPrice_' + sid).val();
    var Subscriptionid = sid

    var dataToPost = { monthdata: monthdatavalue, pricedata: pricedatavalue, planid: Subscriptionid };

    $.ajax({
        type: 'POST',
        url: "/Admin/CustSubscription/UpdatePlan",
        data: dataToPost,
     
        success: function (data) {
            debugger;

            $('#Edit_' + sid).attr("onClick", "EditPlan(" + data.subscriptionSetupID + ")");

            $('#Update_' + sid).attr("onClick", "UpdatePlan(" + data.subscriptionSetupID + ")");

            $('#Cancel_' + sid).attr("onClick", "CancelPlan(" + data.subscriptionSetupID + ")");


            $('#Edit_' + sid).attr("id", 'Edit_' + data.subscriptionSetupID);

            $('#Update_' + sid).attr("id", 'Update_' + data.subscriptionSetupID);

            $('#Cancel_' + sid).attr("id", 'Cancel_' + data.subscriptionSetupID);

           
            $('#originplan_' + sid).attr('id', 'originplan_' + data.subscriptionSetupID);

            $('#editplan_' + sid).attr('id', 'editplan_' + data.subscriptionSetupID);

            $('#UpdateDivanbutton_' + sid).attr('id', 'UpdateDivanbutton_' + data.subscriptionSetupID);
            $('#originplanDivbutton_' + sid).attr('id', 'originplanDivbutton_' + data.subscriptionSetupID);

            swal("Updated", " Subscription Plan updated successfully")

            var durationText = "";
            if (data.durationType === "M") {
                durationText = "Month";
            }
            if (data.durationType === "Y") {
                durationText = "Year";
            }


            $('#originplanduration_' + sid).text("" + data.duration + "" +" "+ durationText);
            $('#originplanprcie_' + sid).text("$" + parseFloat(data.price, 10).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "$1,").toString());

            location.reload();
            // $("#originplanprcie_40123").text('$' + parseFloat(422555, 10).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "$1,").toString());
           
            CancelPlan(data.subscriptionSetupID);
        }
    });

}
