from flask import Flask, render_template, request
from review_scraper import Scraper
from scraper import AmazonReviewScraper
from sentiment_analyzer import SentimentAnalyzer

app = Flask(__name__)


scraper = AmazonReviewScraper()
model = SentimentAnalyzer()
reviews = []

@app.route("/", methods=["GET", "POST"])
def index():
    if request.method == "POST":
        url = request.form.get("url")
        reviews = scraper.get_reviews(url)
        reviews["Sentiment"] = reviews["Description"].apply(lambda x: model.predict_sentiment(x)["sentiment"])
        positive_sentiment_percentage = len(reviews[reviews["Sentiment"] == "Positive"]) / len(reviews["Sentiment"])
        negative_sentiment_percentage = len(reviews[reviews["Sentiment"] == "Negative"]) / len(reviews["Sentiment"])
        
        positive_sentiment_percentage = f"{(positive_sentiment_percentage * 100):.2f}"
        negative_sentiment_percentage = f"{(negative_sentiment_percentage * 100):.2f}"

        return render_template("index.html", positive_sentiment_percentage=positive_sentiment_percentage, negative_sentiment_percentage=negative_sentiment_percentage, reviews=reviews)
    else:
        return render_template("index.html")

if  __name__ == "__main__":
    app.run(debug=True)
