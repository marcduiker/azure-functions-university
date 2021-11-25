import nltk
from nltk.corpus import stopwords
nltk.download("stopwords")


def remove_stop_words(text):
    """
    Removes the English list of stopwords
    (https://gist.github.com/sebleier/554280) from a given text.

    Args:
        text (str): Text to process.

    Returns:
        str: Text with removen stopwords.
    """
    STOPLIST = stopwords.words("english")
    text = " ".join([word for word in text.split() if word not in STOPLIST])
    return text


def tokenize_text(text):
    """
    Tokenizes and tags a given text.

    Args:
        text (str): Text to process.

    Returns:
        List(Tuple[str, str]): List of word-tag pairs.
    """
    nltk.download("punkt")
    nltk.download("averaged_perceptron_tagger")
    tokens = nltk.word_tokenize(text)
    tagged = nltk.pos_tag(tokens)
    return tagged


def get_entities(tagged_text):
    """
    Identifies named entities.

    Args:
        tagged_text (str): Tagged text to process.

    Returns:
        nltk.tree.Tree: A hierarchical grouping of entities.
    """
    nltk.download("maxent_ne_chunker")
    nltk.download("words")
    entities = nltk.chunk.ne_chunk(tagged_text)
    return entities
