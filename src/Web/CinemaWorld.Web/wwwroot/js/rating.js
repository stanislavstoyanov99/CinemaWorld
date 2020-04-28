function showRating(classes) {
    var stars_elements = document.getElementsByClassName(classes);

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

showRating("stars_ratings");

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
            if (data.authenticateErrorMessage != null) {
                let authenticate_error = document.getElementById("error");
                authenticate_error.style.display = "block";
                authenticate_error.innerHTML = data.authenticateErrorMessage;
            }

            if (data.errorMessage != null) {
                let button = document.createElement("button");
                button.setAttribute("type", "button");
                button.setAttribute("data-dismiss", "alert")
                button.className = "close";
                button.innerHTML = "&times;";

                let date = convertUTCDateToLocalDate(new Date(data.nextVoteDate));
                let rating_error = document.getElementById("error");
                rating_error.style.display = "block";

                rating_error.innerHTML = data.errorMessage + " " + date.toLocaleString();
                rating_error.appendChild(button);
            }

            // Update Ratings on all divs
            let elements = document.getElementsByClassName("movie_" + movieId);
            for (let a = 0; a < elements.length; a++) {
                let votes = elements[a].querySelector(".starRatingsSum");
                votes.innerHTML = "(" + data.starRatingsSum + ")";
            }

            showRating("movie_" + movieId);
        }
    });
}

function convertUTCDateToLocalDate(date) {
    var newDate = new Date(date.getTime() + date.getTimezoneOffset() * 60 * 1000);

    var offset = date.getTimezoneOffset() / 60;
    var hours = date.getHours();

    newDate.setHours(hours - offset);

    return newDate;
}