package pokemon_tcg_api

import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.delay
import kotlinx.coroutines.withContext
import kotlinx.serialization.json.Json
import kotlinx.serialization.json.decodeFromJsonElement
import kotlinx.serialization.json.jsonObject
import java.io.*
import java.net.URL
import javax.net.ssl.HttpsURLConnection


class APIHelper {

    companion object {
        private val API_KEY = System.getenv("POKEMON_TCG_API_KEY")
        private val SETS_SAVE_FILE = File(".").canonicalPath + "/src/main/resources/sets.json"
        private val SET_SAVE_FILE_FOLDER = File(".").canonicalPath + "/src/main/resources/sets/"

        suspend fun getCardsFromSet(setId: String): List<Card> = withContext(Dispatchers.IO) {
            val allCards = readCards()
            return@withContext allCards[setId]!!
        }

        private suspend fun readCards(): Map<String, List<Card>> = withContext(Dispatchers.IO) {
            saveCards()
            val cards = mutableMapOf<String, List<Card>>()
            val setFiles = File(SET_SAVE_FILE_FOLDER).listFiles()!!
            val format = Json { ignoreUnknownKeys = true }
            for (file in setFiles) {
                if(file.isFile()){
                    val fileInputStream = FileInputStream(file)
                    val inputStreamReader = InputStreamReader(fileInputStream)
                    val jsonString = inputStreamReader.readText()
                    inputStreamReader.close()
                    fileInputStream.close()

                    val element = Json.parseToJsonElement(jsonString)
                    val data = element.jsonObject["data"]!!

                    val setName = file.nameWithoutExtension
                    cards[setName] = format.decodeFromJsonElement<List<Card>>(data)
                }
            }
            return@withContext cards
        }

        private suspend fun saveCards() {
            File(SET_SAVE_FILE_FOLDER).mkdir()
            val series = readSets()
            val entryList = series.entries.toList()
            var i = 0
            while (i < 1) {
                val sets = entryList[i].value
                var j = 0
                while (j < 1) {
                    val setId = sets[j].id
                    saveCardsFromSet(setId)
                    saveCardImagesForSet(setId)
                    j++
                }
                i++
            }
        }

        private suspend fun saveCardImagesForSet(id: String) = withContext(Dispatchers.IO) {
            File(SET_SAVE_FILE_FOLDER + id).mkdir()
            val fileInputStream = FileInputStream("$SET_SAVE_FILE_FOLDER$id.json")
            val inputStreamReader = InputStreamReader(fileInputStream)
            val jsonString = inputStreamReader.readText()
            inputStreamReader.close()
            fileInputStream.close()

            val format = Json { ignoreUnknownKeys = true }
            val element = Json.parseToJsonElement(jsonString)
            val data = element.jsonObject["data"]!!
            val cards = format.decodeFromJsonElement<List<Card>>(data)
            for (card in cards) {
                val imagePath = SET_SAVE_FILE_FOLDER + id + "/${card.id}"
                saveImages(imagePath, card)
            }
        }

        private suspend fun saveImages(imagePath: String, card: Card) {
            val smallImageURL = card.images["small"]!!
            val largeImageURL = card.images["large"]!!
            requestAndSaveImage(
                "$imagePath-small.png",
                smallImageURL,
                mapOf(Pair("X-Api-Key", API_KEY))
            )
            requestAndSaveImage(
                "$imagePath-large.png",
                largeImageURL,
                mapOf(Pair("X-Api-Key", API_KEY))
            )
        }


        private suspend fun saveCardsFromSet(setId: String) {
            requestAndSaveResponse(
                "$SET_SAVE_FILE_FOLDER$setId.json",
                "https://api.pokemontcg.io/v2/cards?q=set.id:$setId",
                mapOf(Pair("X-Api-Key", API_KEY))
            )
        }

        /**
         * Saves the json response containing the card sets to the resource folder and
         * reads the data into [Series] classes.
         * The [Series]s are then grouped by the series they belong to and are sorted by date.
         *
         * @return A map of the series names to the [Series]s contained in those series.
         */
        private suspend fun readSets(): Map<String, List<Series>> = withContext(Dispatchers.IO) {

            requestAndSaveSets()
            val fileInputStream = FileInputStream(SETS_SAVE_FILE)
            val inputStreamReader = InputStreamReader(fileInputStream)
            val jsonString = inputStreamReader.readText()
            inputStreamReader.close()
            fileInputStream.close()

            val element = Json.parseToJsonElement(jsonString)
            val data = element.jsonObject["data"]!!
            val series = Json.decodeFromJsonElement<List<Series>>(data)

            return@withContext series.groupBy {
                it.series
            }.toList().sortedBy { (_, setList) ->
                setList.minOf { it.releaseDate }
            }.associate { (setName, setList) ->
                Pair(setName, setList.sortedBy { it.releaseDate })
            }
        }

        /**
         * Saves the json response containing the card sets to the resource folder.
         */
        private suspend fun requestAndSaveSets() {
            requestAndSaveResponse(
                SETS_SAVE_FILE,
                "https://api.pokemontcg.io/v2/sets",
                mapOf(Pair("X-Api-Key", API_KEY))
            )
        }

        private suspend fun requestAndSaveResponse(
            fileName: String,
            urlString: String,
            requestProperties: Map<String, String>
        ) = withContext(Dispatchers.IO) {
            if (!File(fileName).exists()) {

                // URL with header
                val url = URL(urlString)
                val connection = url.openConnection() as HttpsURLConnection
                connection.setRequestMethod("GET")
                for ((k, v) in requestProperties) {
                    connection.addRequestProperty(k, v)
                }
                connection.connect()

                // Output file
                val fileOutputStream = FileOutputStream(fileName)
                val outputStreamWriter = OutputStreamWriter(fileOutputStream)

                // Save json response to output file
                val inputStream = InputStreamReader(connection.inputStream)
                val bufferedReader = BufferedReader(inputStream)
                var output: String?
                while (bufferedReader.readLine().also { output = it } != null) {
                    outputStreamWriter.write(output!!)
                }
                outputStreamWriter.flush()

                // Close resources
                bufferedReader.close()
                inputStream.close()
                outputStreamWriter.close()
                fileOutputStream.close()
                connection.disconnect()

                delay(500L)

            }
        }

        private suspend fun requestAndSaveImage(
            fileName: String,
            urlString: String,
            requestProperties: Map<String, String>
        ) = withContext(Dispatchers.IO) {
            if (!File(fileName).exists()) {

                // URL with header
                val url = URL(urlString)
                val connection = url.openConnection() as HttpsURLConnection
                connection.setRequestMethod("GET")
                for ((k, v) in requestProperties) {
                    connection.addRequestProperty(k, v)
                }
                connection.connect()

                val outputStream = FileOutputStream(fileName)
                val inputStream = connection.inputStream

                val buffer = ByteArray(1024)
                var bytesRead: Int
                while (inputStream.read(buffer).also { bytesRead = it } != -1) {
                    outputStream.write(buffer, 0, bytesRead)
                }

                outputStream.flush()

                // Close resources
                inputStream.close()
                outputStream.close()
                connection.disconnect()

                delay(500L)

            }
        }

    }

}