import pika
import requests
import json
import time

# time.sleep(8) # Wait for rabbitmq to start
connection = pika.BlockingConnection(pika.ConnectionParameters('rabbitmq'))
channel = connection.channel()
exchange_name = 'order_exchange'
create_order_routing_key = 'create-order'
email_queue_name = 'email-queue'
email_template = '<h2>Thank you for ordering!</h2><p>We hope you will enjoy our lovely products and don\'t hesitate to contact us if there are any questions.</p><table><thead><tr style="background-color: rgba(155, 155, 155, .2)"><th>Name</th><th>Address</th><th>City</th><th>Zipcode</th><th>Country</th><th>Date of order</th><th>Total price</th></tr></thead><tbody>%s</tbody></table>'

# Declare the exchange, if it doesn't exist
channel.exchange_declare(exchange=exchange_name, exchange_type='direct', durable=True)
# Declare the queue, if it doesn't exist
channel.queue_declare(queue=email_queue_name, durable=True)
# Bind the queue to a specific exchange with a routing key
channel.queue_bind(exchange=exchange_name, queue=email_queue_name, routing_key=create_order_routing_key)

def send_simple_message(to, subject, body):
    return requests.post(
        "https://api.mailgun.net/v3/sandbox6f519ddf01154e43a121a475f12cdad9.mailgun.org/messages",
        auth=("api", "938a4ef013093e15cbc4719f7b544bd0-8845d1b1-5dee0db1"),
        data={"from": "Mailgun Sandbox <postmaster@sandbox6f519ddf01154e43a121a475f12cdad9.mailgun.org>",
              "to": to,
              "subject": subject,
              "html": body})

def send_order_email(ch, method, properties, data):
    parsed_msg = json.loads(data)
    address = parsed_msg['streetName'] + ' ' + parsed_msg['houseNumber']
    info_html = ''.join('<tr><td>%s</td><td>%s</td><td>%s</td><td>%s</td><td>%s</td><td>%s</td><td>%.2f</td></tr>' % (parsed_msg['fullName'], address, parsed_msg['city'], parsed_msg['zipCode'], parsed_msg['country'], parsed_msg['orderDate'], parsed_msg['totalPrice']))
    order_items_template = '<h3>Order items</h3><table><thead><tr style="background-color: rgba(155, 155, 155, .2)"><th>Product name</th><th>Quantity</th><th>Price</th></tr></thead><tbody>%s</tbody></table>'
    order_items_html = ''.join([ '<tr><td>%s</td><td>%.2f</td><td>%.2f</td></tr>' % (item['productIdentifier'], item['quantity'], item['totalPrice']) for item in parsed_msg['orderItems'] ])
    representation = email_template % info_html + order_items_template % order_items_html
    send_simple_message(parsed_msg['email'], 'Successful order!', representation)
    print("Order confirmation has been sent to %s!" % parsed_msg['email'])

channel.basic_consume(on_message_callback=send_order_email,
                      queue=email_queue_name,
                      auto_ack=True)

channel.start_consuming()
connection.close()
