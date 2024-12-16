import requests
from bs4 import BeautifulSoup
import pandas as pd
import csv
from sentiment_analyzer import SentimentAnalyzer

reviewlist = []

class Scraper:
    def __init__(self):
        self.model = SentimentAnalyzer()
    def get_reviews(self, url):
        resp = requests.get(url)
        soup = BeautifulSoup(resp.text, 'html.parser')
        reviews = soup.findAll('div', {'data-hook': 'review'})
        try:
            for item in reviews:
                review = {
                    'product_name': soup.title.text.strip(),
                    'rating_title': item.find('a', {'data-hook': 'review-title'}).contents[3].text.strip(),
                    'rating_value':  float(item.find('i', {'data-hook': 'review-star-rating'}).text.replace('out of 5 stars', '').strip()),
                    'body': item.find('span', {'data-hook': 'review-body'}).text.strip(),
                }
                # Model predicts and prediction is stored in the review dictionary
                prediction = self.model.predict_sentiment(review['body'])
                review['prediction'] = prediction
                reviewlist.append(review)

            # for item in reviewlist:
            #     if item['rating_value'] >= 4.0:
            
        except:
            pass
        return reviewlist

# df = pd.DataFrame(reviewlist)
# df.to_excel('sony-headphones.xlsx', index=False)
# print('Fin.')