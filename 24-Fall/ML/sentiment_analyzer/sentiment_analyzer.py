from transformers import AutoTokenizer, AutoModelForSequenceClassification
import torch

class SentimentAnalyzer:
    def __init__(self):
        # Load the saved model and tokenizer
        MODEL_NAME = "models/Model 3 - Transfer Learning with BERT/bert_sentiment_model"  # Trained BERT model
        self.tokenizer = AutoTokenizer.from_pretrained(MODEL_NAME)
        self.model = AutoModelForSequenceClassification.from_pretrained(MODEL_NAME)

        # Move the model to the appropriate device
        self.device = torch.device("cuda" if torch.cuda.is_available() else "cpu")
        self.model.to(self.device)
        self.model.eval()
    def predict_sentiment(self, review, max_length=128):
        """
        Predict sentiment for a single review.
        
        Args:
            review (str): The input review as a string.
            model: The fine-tuned BERT model.
            tokenizer: The tokenizer corresponding to the BERT model.
            max_length (int): The maximum token length for the review.
        
        Returns:
            dict: The predicted label (0 for negative, 1 for positive) and probabilities.
        """
        # Tokenize the review
        inputs = self.tokenizer(
            review,
            max_length=max_length,
            truncation=True,
            padding="max_length",
            return_tensors="pt"
        )
        
        # Move input tensors to the device
        inputs = {key: val.to(self.device) for key, val in inputs.items()}
        
        # Perform inference
        with torch.no_grad():
            outputs =self.model(**inputs)
            logits = outputs.logits
            probabilities = torch.softmax(logits, dim=1)
            predicted_label = torch.argmax(probabilities, dim=1).item()
        
        # Map label to sentiment
        sentiment = "Positive" if predicted_label == 1 else "Negative"
        return {"sentiment": sentiment, "probabilities": probabilities.cpu().numpy()}
