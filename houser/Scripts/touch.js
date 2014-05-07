/// <reference path="alt.js" />


touch = {};

touch.x = null;
touch.y = null;
touch.swipeDirection = null;

touch.startx = null;
touch.starty = null;

touch.holdX = null;
touch.holdY = null;
touch.hold = null;

touch.left = 1;
touch.right = 2;
touch.up = 3;
touch.down = 4;
touch.hold1 = 5;

touch.getStart = function getStart(evt) {
    touch.startx = evt.touches[0].clientX;
    touch.starty = evt.touches[0].clientY;
}
touch.getSwipeDirection = function getSwipeDirection(evt) {
    if (touch.swipeDirection === null) {

        var x = evt.touches[0].clientX;
        var y = evt.touches[0].clientY;

        var xDiff = 0;
        var yDiff = 0;

        var tempXDiff = x - touch.startx;
        var tempYDiff = y - touch.starty;

        if (Math.abs(tempXDiff) > 30 || Math.abs(tempYDiff) > 30) {
            xDiff = x - touch.startx;
            yDiff = y - touch.starty;
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