from miio.device import Device
from miio.integrations.fan.dmaker.fan_miot import FanMiot
from phue import Bridge
import time
from threading import Timer

from log_utility import log_manipulation_info
from pygame import mixer


class Matter(object):
    pass


# The states argument defines the name of different menus
states = ['init', 'rooms', 'message', 'message list', 'living room', 'kitchen', 'light 1', 'light 2', 'fan', 'music', 'top light',
          'coffee',
          'tea',
          'dishwasher', 'music list']

# The trigger argument defines the name of the new triggering method
transitions = [
    {'trigger': 'up', 'source': 'init', 'dest': 'rooms'},
    {'trigger': 'down', 'source': 'init', 'dest': 'rooms'},
    {'trigger': 'left', 'source': 'init', 'dest': 'rooms'},
    {'trigger': 'right', 'source': 'init', 'dest': 'rooms'},
    {'trigger': 'down', 'source': 'rooms', 'dest': 'kitchen'},
    {'trigger': 'right', 'source': 'rooms', 'dest': 'living room'},
    {'trigger': 'up', 'source': 'rooms', 'dest': 'message'},
    {'trigger': 'left', 'source': 'message', 'dest': 'rooms'},
    {'trigger': 'right', 'source': 'message', 'dest': 'message list'},
    {'trigger': 'right', 'source': 'message list', 'dest': 'message'},
    {'trigger': 'left', 'source': 'rooms', 'dest': 'init'},
    {'trigger': 'up', 'source': 'living room', 'dest': 'light 1'},
    {'trigger': 'down', 'source': 'living room', 'dest': 'music'},
    {'trigger': 'left', 'source': 'living room', 'dest': 'rooms'},
    {'trigger': 'right', 'source': 'living room', 'dest': 'fan'},
    {'trigger': 'right', 'source': 'kitchen', 'dest': 'tea'},
    {'trigger': 'left', 'source': 'kitchen', 'dest': 'rooms'},
    {'trigger': 'left', 'source': 'light 1', 'dest': 'living room'},
    {'trigger': 'left', 'source': 'light 2', 'dest': 'living room'},
    {'trigger': 'left', 'source': 'music', 'dest': 'living room'},
    {'trigger': 'right', 'source': 'music', 'dest': 'music list'},
    {'trigger': 'left', 'source': 'music list', 'dest': 'music'},
    {'trigger': 'right', 'source': 'music list', 'dest': 'music'},
    {'trigger': 'left', 'source': 'fan', 'dest': 'living room'},
    {'trigger': 'left', 'source': 'tea', 'dest': 'kitchen'},
]


class SocketTimer(object):
    def __init__(self, interval, function):
        self._interval = interval
        self._function = function

    def start_new_timer(self):
        socket_timer = Timer(self._interval, self._function)
        socket_timer.start()


class IoTInterface:
    def __init__(self):
        self.angle = 0
        self.prev_angle = 0
        self.fan_state = 0
        self.light_state = 0
        self.socket_state = 0
        self.fan = None
        self.plug = None
        self.light_bridge = None
        self.brightness = 0
        self.fan_speed = 0

    def init_iot_devices(self):
        try:
            self.turn_off_fan()
            self.fan.set_buzzer(False)

        except:
            print("Can't load fan")
            self.fan = None

        try:
            self.turn_off_socket()
        except:
            print("Can't load plug")
            self.plug = None
        try:
            self.turn_off_light()
        except:
            print("Can't load light")
            self.light_bridge = None

    def run(self):
        # self._participant = input("Participant (e.g., p0)?")
        # self._session = input("Session (e.g., 0 or 1)?")
        self._participant = "test"
        self._session = "test"
        self.start_light()
        self.load_IoT_devices()
        self.init_iot_devices()
        self.start_linstener()

    def start_linstener(self):
        import time
        import zmq

        context = zmq.Context()
        socket = context.socket(zmq.REP)
        socket.bind("tcp://*:5555")

        while True:
            #  Wait for next request from client
            try:
                message = socket.recv()
                print("Received request: %s" % message)
                cmd = message.decode('utf-8').split(",")
                self.parse_cmd(cmd)
            except:
                pass

            socket.send(b"World")
            time.sleep(1)

    def parse_cmd(self, cmd):
        print(cmd)
        if cmd[0].__contains__("Light"):
            if cmd[1] == "Increase":
                self.increase_brightness()
            elif cmd[1] == "Decrease":
                self.decrease_brightness()
            elif cmd[1] == "Brightness":
                self.set_light_brightness(int(cmd[2]))
            elif cmd[1].title() == "On":
                self.turn_on_light()
            else:
                self.turn_off_light()
        elif cmd[0].__contains__("Fan"):
            if cmd[1] == "Increase":
                self.increase_speed()
            elif cmd[1] == "Decrease":
                self.decrease_speed()
            elif cmd[1] == "Speed":
                self.set_fan_speed(int(cmd[2]))
            elif cmd[1] == "On":
                self.turn_on_fan()
            else:
                self.turn_off_fan()
        elif cmd[0].__contains__("Timer"):
            if cmd[1] == "Start":
                self.turn_on_socket()
            else:
                self.turn_off_socket()
        elif cmd[0].__contains__("Music"):
            if cmd[1] == "Play":
                self.unpause_music()
            elif cmd[1] == "Stop":
                self.pause_music()
            else:
                self.select_music(cmd[2])

    def load_IoT_devices(self):
        try:
            self.fan = FanMiot('192.168.31.6', '96efb75b817348d486e8b33ae3b8252d')
        except:
            print("Can't load fan")
        try:
            self.plug = Device('192.168.31.15', 'e6edc1993dc849482636d4b29bb24e70')
        except:
            print("Can't load plug")
        try:
            self.light_bridge = Bridge('192.168.31.121')
            self.start_light()
        except:
            print("Can't load light")

    def start_light(self):
        if self.light_bridge is None:
            return
        # If the app is not registered and the button is not pressed, press the button and call connect()
        self.light_bridge.connect()

        # Get the bridge state (This returns the full dictionary that you can explore)
        self.light_bridge.get_api()

    def switch_light(self):
        if self.light_bridge is None:
            return
        print("state", self.light_state)
        if self.light_state == 0:
            self.turn_on_light()
        else:
            self.turn_off_light()

    def turn_off_light(self):
        if self.light_bridge is None:
            return
        self.light_state = 0
        self.light_bridge.set_light(2, 'on', False)
        self.brightness = 0
        print("turn off light")

    def turn_on_light(self, brightness=127):
        if self.light_bridge is None:
            return
        self.light_state = 1
        self.light_bridge.set_light(2, 'on', True)
        self.light_bridge.set_light(2, 'bri', brightness)
        self.brightness = 50
        print("turn on light")

    def increase_brightness(self):
        self.brightness += 10
        self.set_light_brightness(self.brightness)

    def decrease_brightness(self):
        self.brightness -= 10
        self.set_light_brightness(self.brightness)

    def increase_speed(self):
        self.fan_speed += 10
        self.set_fan_speed(self.fan_speed)

    def decrease_speed(self):
        self.fan_speed -= 10
        self.set_fan_speed(self.fan_speed)

    def set_light_brightness(self, brightness):
        if self.light_bridge is None:
            return
        if brightness == 0:
            self.turn_off_light()
            return

        if self.light_state == 0:
            self.light_bridge.set_light(2, 'on', True)

        self.light_bridge.set_light(2, 'bri', int(brightness / 100 * 254))
        self.light_state = 1
        self.brightness = brightness
        print("light", self.light_state, int(brightness / 100 * 254))

    def turn_on_socket(self):
        if self.plug is None:
            return
        self.socket_state = 1
        self.plug.send("set_properties", [{'did': 'MYDID', 'siid': 2, 'piid': 1, 'value': True}])
        print("turn on the socket")
        socket_timer = SocketTimer(280, self.turn_on_socket)
        socket_timer.start_new_timer()

    def turn_off_socket(self):
        if self.plug is None:
            return
        self.socket_state = 0
        self.plug.send("set_properties", [{'did': 'MYDID', 'siid': 2, 'piid': 1, 'value': False}])
        print("turn off the socket")

    def switch_socket(self):
        if self.plug is None:
            return
        if self.socket_state == 0:
            self.turn_on_socket()
        else:
            self.turn_off_socket()

    def turn_on_fan(self):
        if self.fan is None:
            return
        self.fan_state = 1
        self.fan.on()
        self.fan_speed = 50
        print("turn on the fan")

    def turn_off_fan(self):
        if self.fan is None:
            return
        self.fan_state = 0
        self.fan.off()
        self.fan_speed = 0
        print("turn off the fan")

    def set_fan_speed(self, speed):
        if self.fan is None:
            return
        if speed != 0:
            if self.fan_state == 0:
                self.fan.on()
                self.fan.set_speed(int(speed))
                self.fan_speed = speed
            print('Update Speed: ', int(speed))
        else:
            self.turn_off_fan()

    def switch_fan(self):
        if self.fan is None:
            return
        if self.fan_state == 0:
            self.turn_on_fan()
            self.fan_speed = 50
        else:
            self.turn_off_fan()
            self.fan_speed = 0

    def select_music(self, music):
        mixer.init()
        # --------------------------Path of your music
        if music.__contains__("Hello"):
            mixer.music.load("data/music/Adele-Hello.mp3")
        elif music.__contains__("Hotel"):
            mixer.music.load("data/music/Eagles - hotel califorlia.mp3")
        elif music.__contains__("Suds"):
            mixer.music.load("data/music/Sara Evans-Suds In The Bucket.mp3")
        elif music.__contains__("Attention"):
            mixer.music.load("data/music/Attention - Charlie Puth.mp3")
        elif music.__contains__("Receives"):
            mixer.music.load("data/music/Keith Kenniff-Receives.mp3")
        elif music.__contains__("Lover"):
            mixer.music.load("data/music/Taylor_Swift-Lover.mp3.mp3")
        elif music.__contains__("Hold On"):
            mixer.music.load("data/music/Justin_Bieber_Hold_On.mp3")
        elif music.__contains__("Time"):
            mixer.music.load("data/music/Sophie Zelmani-Time To Kill.mp3")
        mixer.music.set_volume(0.5)
        mixer.music.play()

    def unpause_music(self):
        mixer.music.unpause()

    def pause_music(self):
        mixer.music.pause()


if __name__ == '__main__':
    iot = IoTInterface()
    iot.run()
