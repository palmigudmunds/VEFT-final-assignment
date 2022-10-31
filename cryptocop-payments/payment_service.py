import pika
import json
from credit_card_checker import CreditCardChecker

connection = pika.BlockingConnection(pika.ConnectionParameters('rabbitmq'))
channel = connection.channel()
exchange_name = 'order_exchange'
create_order_routing_key = 'create-order'
email_queue_name = 'payment-queue'

# Declare the exchange, if it doesn't exist
channel.exchange_declare(exchange=exchange_name, exchange_type='direct', durable=True)
# Declare the queue, if it doesn't exist
channel.queue_declare(queue=email_queue_name, durable=True)
# Bind the queue to a specific exchange with a routing key
channel.queue_bind(exchange=exchange_name, queue=email_queue_name, routing_key=create_order_routing_key)

def validate_credit_card(ch, method, properties, data):
    parsed_msg = json.loads(data)
    credit_card = parsed_msg['creditCard']

    if CreditCardChecker(credit_card).valid():
        print("Credit card is valid!")
    else:
        print("Credit card is invalid!")

channel.basic_consume(on_message_callback=validate_credit_card,
                      queue=email_queue_name,
                      auto_ack=True)

channel.start_consuming()
connection.close()
