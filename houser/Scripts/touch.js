/// <reference path="alt.js" />


touch = {};

touch.xDown = null;
touch.yDown = null;

touch.left = 1;
touch.right = 2;
touch.up = 3;
touch.down = 4;

touch.handleTouchStart = function handleTouchStart(evt) {
    touch.xDown = evt.touches[0].clientX;
    touch.yDown = evt.touches[0].clientY;
};

touch.swipeDirection = function swipeDirection(evt) {
    if (!touch.xDown || !touch.yDown) {
        return;
    }

    var x = evt.touches[0].clientX;
    var y = evt.touches[0].clientY;

    var xDiff = 0;
    var yDiff = 0;
    var xOverY = 0;
    var flag = 0;


    if (alt.x === null) {
        flag = 1;
        alt.x = evt.touches[0].clientX;
        alt.y = evt.touches[0].clientY;
    }
    if (alt.x !== null && flag === 0) {
        xDiff = x - alt.x;
        yDiff = y - alt.y;
    }

    if (Math.abs(xDiff) > Math.abs(yDiff)) {/*most significant*/
        if (xDiff > 0) {
            return touch.left;
        } else if (xDiff < 0) {
            return touch.right;
        } else {
            return null;
        }
    } else {
        if (yDiff > 0) {
            return touch.up;
        } else if (yDiff < 0) {
            return touch.down;
        } else {
            return null;
        }
    }
    touch.xDown = null;
    touch.yDown = null;
};