$('#contact-submit').click(function (event) {

    event.preventDefault();

    //Get values from the contact form
    const name = document.getElementById("name").value;
    const email = document.getElementById("email").value;
    const subject = document.getElementById("subject").value;
    const message = document.getElementById("message").value;

    //Patterns to validate fields
    const namePattern = /^[a-zA-Z -]+$/;
    const emailPattern = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    const subjectPattern = /^[a-zA-Z0-9 .,_-]+$/;
    const messagePattern = /^[a-zA-Z0-9., !?()'\r\n]+$/;

    const errorIdList = ["name-empty", "name-invalid", "email-empty", "email-invalid",
        "subject-empty", "subject-invalid", "message-empty", "message-invalid"];

    let grecaptchaIsValid = true;
    let nameIsValid = true;
    let emailIsValid = true;
    let subjectIsValid = true;
    let messageIsValid = true;

    resetErrorMessages(errorIdList);

    //Validate each field
    nameIsValid = validateField(name, "name-empty", "name-invalid", 1);
    emailIsValid = validateField(email, "email-empty", "email-invalid", 2);
    subjectIsValid = validateField(subject, "subject-empty", "subject-invalid", 3);
    messageIsValid = validateField(message, "message-empty", "message-invalid", 4);

    //Validate reCAPTCHA
    grecaptcha.ready(function () {
        grecaptcha.execute('6Lfwx9olAAAAAJUU9QY63FFUOmFj2ehvZS9ROwP-', { action: 'submit' })
            .then(function (token) {
                $.getJSON("/Home/RecaptchaV3Vverify?token=" + token,
                    function (data) {
                        grecaptchaIsValid = data.success;
                        console.log(data);
                        sentIfFieldsAreTrue();
                    });
            });
    });


    //Send message to controller if all isValid is true
    //if (!nameIsValid || !emailIsValid || !subjectIsValid || !messageIsValid || !grecaptchaIsValid) {
    //    console.log("Invalid Message")
    //} else {
    //    sendMessage(name, email, subject, message);
    //}

    function sentIfFieldsAreTrue() {
        if (!nameIsValid || !emailIsValid || !subjectIsValid || !messageIsValid || !grecaptchaIsValid) {
            console.log("Invalid Message")
        } else {
            sendMessage(name, email, subject, message);

        }
    }


    //Sends message to the controller /Home/SendMail
    function sendMessage(name, email, subject, message) {

        $.ajax({
            type: 'POST',
            url: '/Home/SendMail',
            data: {
                name: name,
                email: email,
                subject: subject,
                message: message
            },
            headers: {
                RequestVerificationToken:
                    document.getElementById("RequestVerificationToken").value
            },
            success: function (response) {
                console.log(response)
                displaySuccessOrError(response);
            },
            error: function (err) {
                console.log(err.responseText);
                let error = document.getElementById("error");
                displayMessageSmoothly(error);
                hideMessageSmoothly(error, 5000);
            }
        })
    }

    //Displays success or error after sending the message
    function displaySuccessOrError(response) {
        if (response.success) {
            let success = document.getElementById("success");
            displayMessageSmoothly(success);
            document.getElementById("contact-form").reset();
            hideMessageSmoothly(success, 5000);
        } else {
            let error = document.getElementById("error");
            displayMessageSmoothly(error);
            hideMessageSmoothly(error, 5000);
        }
    }

    //Resets the empty and invalid messages
    function resetErrorMessages(errorIdList) {
        errorIdList.forEach((element) => {
            let errorMessage = document.getElementById(element);
            errorMessage.style.display = "none";
        })
    }

    //Validate if a field isn't null or if it contains unauthorized characters
    function validateField(field, fieldEmpty, fieldInvalid, position) {
        let validField = true;

        if (!field) {
            validField = false;
            let fieldError = document.getElementById(fieldEmpty);
            displayMessageSmoothly(fieldError);
        } else if (!fieldContainsAutorizedChars(field, getPattern(position))) {
            validField = false;
            displayFieldInvalid(fieldInvalid);
        }
        return validField;
    }

    //Return a pattern based on the position in the form
    function getPattern(position) {
        switch (position) {
            case 1:
                return namePattern;
            case 2:
                return emailPattern;
            case 3:
                return subjectPattern;
            case 4:
                return messagePattern;
        }
    }

    //Displays the invalid field message
    function displayFieldInvalid(fieldInvalid) {
        let fieldError = document.getElementById(fieldInvalid);
        displayMessageSmoothly(fieldError);
    }

    //Let you know if a field contains the authorized characters
    function fieldContainsAutorizedChars(field, pattern) {
        return pattern.test(field);
    }

    //Smooth transition - Hide
    function hideMessageSmoothly(message, time) {
        setTimeout(function () {
            message.style.opacity = 0;
            setTimeout(function () {
                message.style.display = "none";
            }, 200);
        }, time);
    }

    //Smooth transition - Display
    function displayMessageSmoothly(message) {
        message.style.opacity = 0;
        message.style.display = "block";
        setTimeout(function () {
            message.style.opacity = 1;
        }, 200);
    }
});
