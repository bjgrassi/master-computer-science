# **Sentiment Analysis of Product Reviews**

This repository contains the implementation of a sentiment analysis system designed to classify e-commerce product reviews as positive or negative. The project combines machine learning, transfer learning, and web development techniques to provide an end-to-end solution for real-time sentiment analysis.

Original Source: https://github.com/k3v1n-KI/semtement_analyzer 
---

## **Overview**

Understanding customer sentiment in product reviews is crucial for businesses and consumers. This project aims to address this challenge by:
1. Developing an initial model using a Recurrent Neural Network (RNN) for sentiment classification.
2. Improving accuracy using transfer learning with a pre-trained BERT model.
3. Deploying the final model in a Flask-based web application that provides real-time predictions for user-input Amazon product URLs.

---

## **Technologies Used**

### **Programming Language**
- Python

### **Machine Learning Libraries**
- TensorFlow
- PyTorch
- Hugging Face Transformers

### **Web Development**
- Flask

### **Web Scraping**
- BeautifulSoup
- Scrapy

### **Visualization**
- Matplotlib
- Seaborn

---

## **Project Features**

1. **Data Collection**  
   - Datasets of reviews were collected from Amazon, Yelp, and IMDb.
   - Additional reviews were scraped using Python tools.

2. **Preprocessing**  
   - Text data was cleaned using tokenization, stop word removal, and lemmatization.

3. **Model Development**  
   - A custom RNN was initially trained but achieved low accuracy (~60%).  
   - A pre-trained BERT model was fine-tuned, achieving 92% accuracy.

4. **Web App Deployment**  
   - A Flask-based app was developed where users can input an Amazon product URL.  
   - The app scrapes reviews from the product page and performs real-time sentiment analysis.

---

## **Model Performance**

| Metric         | RNN        | BERT       |
|----------------|------------|------------|
| Accuracy       | ~60%       | 92.18%     |
| Precision      | -          | 89.45%     |
| Recall         | -          | 95.64%     |
| F1-Score       | -          | 92.45%     |

---

## **How to Use**

### **Prerequisites**
- Python 3.8 or later
- Install required libraries using the following:
  ```bash
  pip install -r requirements.txt
  ```

### **Run the Web App**
1. Clone the repository:
   ```bash
   git clone https://github.com/username/sentiment-analysis
   cd sentiment-analysis
   ```
2. Run the Flask app:
   ```bash
   python app.py
   ```
3. Open your browser and navigate to `http://127.0.0.1:5000`.

4. Input an Amazon product URL to get a sentiment analysis report.


## **Future Work**

- Mitigate overfitting in the BERT model using regularization techniques.
- Expand the dataset to include reviews from more diverse domains.
- Explore more advanced transfer learning models like GPT or RoBERTa.

---

## **Authors**

- **Kevin Igweh**  
- **Bruna Jacinto Grassi**

---

## **Acknowledgments**

We thank **Dr. Ajmery Sultana** for her guidance and support throughout this project.

---
