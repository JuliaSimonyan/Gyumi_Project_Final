document.addEventListener('DOMContentLoaded', function() {
    const backBtn = document.getElementById('backBtn');
    const downloadBtn = document.getElementById('downloadBtn');
    const emailBtn = document.getElementById('emailBtn');
    const shareBtn = document.getElementById('shareBtn');
    const shareModal = document.getElementById('shareModal');
    const closeModal = document.querySelector('.close-modal');
    const copyLinkBtn = document.getElementById('copyLink');
    const shareLinkInput = document.getElementById('shareLink');
    
    backBtn.addEventListener('click', function() {
        window.history.back();
    });
    
    downloadBtn.addEventListener('click', function() {
        const printWindow = window.open('', '_blank');
        const contentToPrint = document.getElementById('plan-content').innerHTML;
        
        printWindow.document.write(`
            <html>
            <head>
                <title>Trip to Gyumri</title>
                <style>
                    body {
                        font-family: Arial, sans-serif;
                        padding: 20px;
                        max-width: 800px;
                        margin: 0 auto;
                    }
                    .plan-header {
                        display: flex;
                        justify-content: space-between;
                        align-items: flex-start;
                        margin-bottom: 30px;
                    }
                    .logo {
                        width: 40px;
                        height: 40px;
                    }
                    .address {
                        text-align: right;
                        font-size: 12px;
                        color: #666;
                    }
                    .plan-title {
                        text-align: center;
                        margin-bottom: 20px;
                    }
                    .plan-title h1 {
                        font-size: 24px;
                        font-weight: 600;
                        margin-bottom: 5px;
                    }
                    .plan-details {
                        display: flex;
                        justify-content: center;
                        gap: 20px;
                        margin-bottom: 30px;
                        padding-bottom: 20px;
                        border-bottom: 1px solid #eee;
                    }
                    .plan-section {
                        margin-bottom: 25px;
                    }
                    .plan-section h2 {
                        font-size: 18px;
                        font-weight: 600;
                        margin-bottom: 15px;
                    }
                    .section-item {
                        display: flex;
                        align-items: center;
                        margin-bottom: 15px;
                        padding-bottom: 15px;
                        border-bottom: 1px solid #f5f5f5;
                    }
                </style>
            </head>
            <body>
                ${contentToPrint}
                <script>
                    window.onload = function() {
                        window.print();
                        window.setTimeout(function() {
                            window.close();
                        }, 500);
                    }
                </script>
            </body>
            </html>
        `);
        
        printWindow.document.close();
    });
    
    emailBtn.addEventListener('click', function() {
        const subject = encodeURIComponent('My Trip to Gyumri');
        const body = encodeURIComponent(`
            Hello,

            I'm excited to share my upcoming trip to Gyumri with you!

            Trip Details:
            - Dates: Mar 19 - Mar 21
            - Accommodation: Berlin Art Hotel
            - Activities: Aregak Bakery & Café, Dzitoghtsyan Museum, Gwoog Gastrohouse

            You can view the full itinerary at: https://gyumritourism.com/trip/12345

            Best regards,
            [Your Name]
        `);
        
        window.location.href = `mailto:?subject=${subject}&body=${body}`;
    });
    
    // Share modal functionality
    if (shareBtn && shareModal) {
        shareBtn.addEventListener('click', function() {
            shareModal.style.display = 'flex';
        });
        
        if (closeModal) {
            closeModal.addEventListener('click', function() {
                shareModal.style.display = 'none';
            });
        }
        
        window.addEventListener('click', function(event) {
            if (event.target === shareModal) {
                shareModal.style.display = 'none';
            }
        });
    }
    
    if (copyLinkBtn && shareLinkInput) {
        copyLinkBtn.addEventListener('click', function() {
            shareLinkInput.select();
            document.execCommand('copy');
            
            copyLinkBtn.textContent = 'Copied!';
            setTimeout(function() {
                copyLinkBtn.textContent = 'Copy';
            }, 2000);
        });
    }
    
    const socialIcons = document.querySelectorAll('.social-icon');
    socialIcons.forEach(icon => {
        icon.addEventListener('click', function(e) {
            e.preventDefault();
            
            const url = encodeURIComponent('https://gyumritourism.com/trip/12345');
            const title = encodeURIComponent('My Trip to Gyumri');
            
            let shareUrl = '';
            
            if (this.classList.contains('facebook')) {
                shareUrl = `https://www.facebook.com/sharer/sharer.php?u=${url}`;
            } else if (this.classList.contains('twitter')) {
                shareUrl = `https://twitter.com/intent/tweet?url=${url}&text=${title}`;
            } else if (this.classList.contains('whatsapp')) {
                shareUrl = `https://api.whatsapp.com/send?text=${title}%20${url}`;
            } else if (this.classList.contains('telegram')) {
                shareUrl = `https://t.me/share/url?url=${url}&text=${title}`;
            }
            
            if (shareUrl) {
                window.open(shareUrl, '_blank', 'width=600,height=400');
            }
        });
    });
});