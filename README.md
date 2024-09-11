


# Pixel Plus

Pixel Plus is a Windows Forms-based image processing tool written in C#. It allows users to apply various transformations, filters, and operations on images, such as color space conversions, histogram enhancements, and morphological operations.

## Features

![PP](https://github.com/user-attachments/assets/27cf908d-53de-4d56-8a6a-558d91ba5226)

### 1. Image Upload
- **Upload First Image**: Load the first image in RGB format.
- **Upload Second Image**: Load the second image and convert it to the HSI color space.

### 2. Binary and Thresholding Operations
- **Binary Conversion**: Convert the image to a binary (black and white) format.
- **Single Thresholding**: Apply single thresholding to segment the image based on intensity values.

### 3. Zoom and Cropping
- **Zoom In**: Magnify a part of the image.
- **Zoom Output**: Reset the zoom level.
- **Image Cropping**: Crop the image to predefined aspect ratios:
  - 4:3
  - 16:9
  - 3:2
  - 1:1

### 4. Edge Detection
- **Prewitt Edge Finding**: Detect edges in the image using the Prewitt operator.

### 5. Arithmetic Operations
- **Addition, Subtraction, Multiplication, and Division**: Perform arithmetic operations on two images.

### 6. Histogram Operations
- **Histogram Stretching**: Enhance the contrast by stretching the intensity levels.
- **Histogram Expansion**: Expand the intensity values for better contrast.
- **Original Histogram**: View the original histogram of the image.

### 7. Filters and Noise Removal
- **Unsharp Masking**: Sharpen the image using the unsharp masking technique.
- **Mean Median Filter**: Apply mean or median filters for noise reduction.
- **Salt & Pepper Noise Removal**: Remove salt and pepper noise from the image.

### 8. Image Enhancements
- **Contrast Enhancement**: Increase the contrast of the image.
- **Contrast Reduction**: Reduce the contrast of the image.

### 9. Morphological Operations
- **Erosion**: Perform morphological erosion to shrink the foreground object.
- **Dilation**: Perform morphological dilation to enlarge the foreground object.
- **Opening**: Remove small objects from the foreground (useful for noise removal).
- **Closing**: Fill small holes in the foreground object.

### 10. Image Processing Operations
- **Color Space Transformations**: Convert between different color spaces:
  - RGB ↔ HSV
  - RGB ↔ HSI
  - RGB ↔ XYZ
  - XYZ ↔ LUV
  - Gray ↔ RGB
  - HSI ↔ RGB
- **Grey Transformation**: Convert the image to grayscale.

### 11. Reset Button
- Reset all transformations and revert the image to its original state.

## How to Run

1. Clone the repository or download the project files.
2. Open the project in Visual Studio.
3. Build the solution and run the program.
4. Upload images and apply the desired transformations and operations using the buttons and options provided.

## Notes

- This project is intended for educational purposes and provides basic image processing operations using C# and Windows Forms.
- Make sure the images you upload are in supported formats (e.g., PNG, JPEG).

