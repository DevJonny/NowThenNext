// Image resizing functionality
window.resizeImage = function(base64String, contentType, maxWidth) {
    return new Promise((resolve, reject) => {
        const img = new Image();
        
        img.onload = function() {
            const canvas = document.createElement('canvas');
            const ctx = canvas.getContext('2d');
            
            // Calculate new dimensions while maintaining aspect ratio
            let { width, height } = img;
            
            if (width > maxWidth) {
                const ratio = maxWidth / width;
                width = maxWidth;
                height = height * ratio;
            }
            
            // Set canvas dimensions
            canvas.width = width;
            canvas.height = height;
            
            // Draw the resized image
            ctx.drawImage(img, 0, 0, width, height);
            
            // Convert back to base64
            const resizedBase64 = canvas.toDataURL(contentType, 0.9);
            resolve(resizedBase64);
        };
        
        img.onerror = function() {
            reject(new Error('Failed to load image'));
        };
        
        img.src = `data:${contentType};base64,${base64String}`;
    });
};

// File input trigger functionality
window.triggerFileInput = function(elementId) {
    try {
        const element = document.getElementById(elementId);
        if (element) {
            element.click();
            return true;
        } else {
            console.error(`Element with id '${elementId}' not found`);
            return false;
        }
    } catch (error) {
        console.error('Error triggering file input:', error);
        return false;
    }
};
