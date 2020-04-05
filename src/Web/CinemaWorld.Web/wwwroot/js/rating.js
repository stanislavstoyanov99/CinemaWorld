function showRating() {
    var stars_elements = document.getElementsByClassName("stars_ratings");

    for (let a = 0; a < stars_elements.length; a++) {
        let rating_number = stars_elements[a].querySelector("div").textContent;

        rating_number = rating_number.replace("(", "");
        rating_number = rating_number.replace(")", "");

        let stars = stars_elements[a].getElementsByTagName("i");

        // Algorithm
        let counter = 0;
        rating_number = rating_number / 10;

        while (rating_number >= 1) {
            stars[counter].className = "fa fa-star";
            counter++;
            rating_number--;
        }

        if (rating_number >= 0.5) {
            stars[counter].className = "fa fa-star-half-full";
        }
    }
}

showRating();

function sendRating(movieId, rating) {
    var token = $("#starRatingsForm input[name='__RequestVerificationToken']").val();
    var json = { movieId: movieId, rating: rating };

    $.ajax({
        url: "/api/ratings",
        type: "POST",
        data: JSON.stringify(json),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        headers: { 'X-CSRF-TOKEN': token },
        success: function (data) {
            $("#starRatingsSum_" + movieId).html("(" + data.starRatingsSum + ")");
            if (data.errorMessage != null) {
                $("#ratingError").html(data.errorMessage);
            }

            showRating();
        }
    });
}