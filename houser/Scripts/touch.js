/// <reference path="alt.js" />


touch = {};

touch.x = null;
touch.y = null;
touch.swipeDirection = null;

touch.holdX = null;
touch.holdY = null;
touch.hold = null;

touch.left = 1;
touch.right = 2;
touch.up = 3;
touch.down = 4;
touch.hold1 = 5;


touch.getSwipeDirection = function getSwipeDirection(evt) {
    if (touch.swipeDirection === null) {
        setTimeout(function () {
            touch.swipeDirection = null;
            touch.x = null;
            touch.y = null;
        }, 100);

        var x = evt.touches[0].clientX;
        var y = evt.touches[0].clientY;

        var xDiff = 0;
        var yDiff = 0;
        var xOverY = 0;
        var flag = 0;


        if (touch.x === null) {
            flag = 1;
            touch.x = evt.touches[0].clientX;
            touch.y = evt.touches[0].clientY;
        }
        if (touch.x !== null && flag === 0) {
            var tempXDiff = x - touch.x;
            var tempYDiff = y - touch.y;
            
            if (Math.abs(tempXDiff) > 30 || Math.abs(tempYDiff) > 30) {
                xDiff = x - touch.x;
                yDiff = y - touch.y;
            }
        }

        if (Math.abs(xDiff) > Math.abs(yDiff)) {/*most significant*/
            if (xDiff > 0) {
                return touch.swipeDirection = touch.left;
            } else if (xDiff < 0) {
                return touch.swipeDirection = touch.right;
            } else {
                return null;
            }
        } else {
            if (yDiff > 0) {
                return touch.swipeDirection = touch.up;
            } else if (yDiff < 0) {
                return touch.swipeDirection = touch.down;
            } else {
                return null;
            }
        }
    }
};