


document.addEventListener('DOMContentLoaded', function () {
    let currentIndex = 0;
    const questions = document.querySelectorAll('.caja-de-preguntas');
    const prevBtn = document.getElementById('prevBtn');
    const nextBtn = document.getElementById('nextBtn');

  
    function actualizarRespuestaDisplay() {
        questions.forEach((q, index) => {
            q.style.display = index === currentIndex ? 'block' : 'none';
        });
        prevBtn.disabled = currentIndex === 0;
        nextBtn.disabled = currentIndex === questions.length - 1;
    }

   
    prevBtn.addEventListener('click', () => {
        if (currentIndex > 0) {
            currentIndex--;
            actualizarRespuestaDisplay();
        }
    });

    nextBtn.addEventListener('click', () => {
        if (currentIndex < questions.length - 1) {
            currentIndex++;
            actualizarRespuestaDisplay();
        }
    });

   
    function mostrarModal(modalId, content = '') {
        const modal = document.getElementById(modalId);
        if (content) {
            const answerElement = modal.querySelector('#answerText');
            if (answerElement) answerElement.textContent = content;
        }
        modal.style.display = 'flex';

        modal.addEventListener('click', (e) => {
            if (e.target === modal) hideModals();
        });

        modal.querySelectorAll('.modal-close').forEach(btn => {
            btn.onclick = hideModals;
        });
    }

    function hideModals() {
        document.querySelectorAll('.modal-drop').forEach(m => {
            m.style.display = 'none';
        });
    }

    // Manejador de respuestas
    document.querySelectorAll('.show-answer').forEach(btn => {
        btn.addEventListener('click', async function () {
            const questionBox = this.closest('.caja-de-preguntas');
            const questionId = this.dataset.id;
            const displayType = questionBox.querySelector('.display-type').value;

            try {
                const response = await fetch(`/api/Respuestas/answer/${questionId}`);
                if (!response.ok) throw new Error('Error en la respuesta');
                const data = await response.json();

                switch (displayType) {
                    case 'Popup':
                        mostrarModal('modalRespuesta', data.answer);
                        break;
                    case 'NuevaVentana':
                        const newWindow = window.open('', '_blank');
                        newWindow.document.write(`<pre>${data.answer}</pre>`);
                        break;
                    case 'TextoEnPantalla':
                        questionBox.querySelector('.respuesta-input').value = data.answer;
                        break;
                    case 'Email':
                        mostrarModal('emailModal');
                        break;
                }
            } catch (error) {
                console.error('Error:', error);
                mostrarModal('modalRespuesta', '⚠️ Error al cargar la respuesta');
            }
        });
    });

    // Envío de email
    document.getElementById('sendEmailBtn').addEventListener('click', async () => {
        const email = document.getElementById('emailInput').value;
        const answer = document.getElementById('answerText').textContent;

        try {
            const response = await fetch('/api/Respuestas/send-email', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ email, answer })
            });

            if (!response.ok) throw new Error('Error en el envío');
            hideModals();
            mostrarModal('modalRespuesta', '✉️ Email enviado correctamente');
        } catch (error) {
            console.error('Error:', error);
            mostrarModal('modalRespuesta', '⚠️ Error al enviar el email');
        }
    });

    // Guardar respuestas
    document.getElementById('salvar-Btn').addEventListener('click', async () => {
        const answers = Array.from(document.querySelectorAll('.respuesta-input')).map(input => ({
            questionId: input.dataset.id,
            response: input.value
        }));

        try {
            const response = await fetch('/api/Respuestas/guardararchivo', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(answers)
            });

            if (!response.ok) throw new Error('Error en guardado');
            showModal('modalRespuesta', '💾 Respuestas guardadas');
        } catch (error) {
            console.error('Error:', error);
            showModal('modalRespuesta', '⚠️ Error al guardar');
        }
    });

  

    document.getElementById('cargar-Btn').addEventListener('click', () => {
        
        const fileInput = document.createElement('input');
        fileInput.type = 'file';
        fileInput.accept = '.txt';

        fileInput.onchange = async (e) => {
            const file = e.target.files[0];
            if (!file) return;

            const formData = new FormData();
            formData.append('file', file);

            try {
               
                const uploadResponse = await fetch('/api/Respuestas/uploadfile', {
                    method: 'POST',
                    body: formData
                });

                if (!uploadResponse.ok) throw new Error('Error en la subida');

                
                const loadResponse = await fetch('/api/Respuestas/cargararchivo');
                if (!loadResponse.ok) throw new Error('Error en carga');

                const answers = await loadResponse.json();
                answers.forEach(({ questionId, response }) => {
                    const input = document.querySelector(`.respuesta-input[data-id="${questionId}"]`);
                    if (input) input.value = response;
                });

                mostrarModal('modalRespuesta', '📂 Archivo TXT cargado y respuestas actualizadas');
            } catch (error) {
                console.error('Error:', error);
                mostrarModal('modalRespuesta', `
                            ⚠️ Error en la carga del archivo:
                            ${error.message || 'Formato de archivo inválido'}
        
                            Asegúrate que el archivo:
                            1. Sea un TXT válido
                            2. Tenga el formato: ID|Respuesta
                            3. Use codificación UTF-8
                            `);
                document.getElementById('uploadStatus').style.display = 'none';
            }
        };

        fileInput.click();
    });

    
    actualizarRespuestaDisplay();
});